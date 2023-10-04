using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using DG.Tweening;

namespace NewGame
{
    public class FollowPath : MonoBehaviour
    {
        public static FollowPath Instance;

        public Transform pathCreators;
        public Transform lines;
        public Transform fills;

        EndOfPathInstruction endOfPathInstruction;

        public Color selectColor;
       public  Color[] defaultColor;
        public Color[] myColors = new Color[3];

        PathCreator currentPath;

        ParticleSystem ps;
        ParticleSystem.EmissionModule emissionModule;

        int indexPath = -1, oneBonus = 0;
        bool isDone, isTouch, autoBrush, canReload = true, isOutSide, busy;
        float distanceTravelled, speed = 8f, timeOutside;
        float timeTouch;

        Vector3 offset, target;
        bool moveEnd;
        bool isMouseDrag;

        Coroutine effectStrip = null;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Color cGray = Color.gray;
            cGray.a = 0.2f;
            defaultColor = new Color[fills.childCount];

            for (int i = 0; i < pathCreators.childCount; i++)
            {
                pathCreators.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().color = cGray;
                pathCreators.GetChild(i).gameObject.SetActive(false);
            }

            lines.gameObject.SetActive(false);
            for (int i = 0; i < lines.childCount; i++)
            {
                pathCreators.GetChild(i).GetChild(0).gameObject.AddComponent<FadeIn>();
            }

            for (int i = 0; i < fills.childCount; i++)
            {
                defaultColor[i] = fills.GetChild(i).GetComponent<SpriteRenderer>().color;
                fills.GetChild(i).gameObject.SetActive(false);
                fills.GetChild(i).GetComponent<SpriteMask>().enabled = false;
                fills.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
                fills.GetChild(i).gameObject.AddComponent<TriggerFillManager>();
            }

            StartCoroutine(IsMouseDrag());

            fills.gameObject.SetActive(true);

            DataManager.IsPointerOverGameObject = Switch.OFF;
            PenRotate.Instance.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;

            currentPath = pathCreators.GetChild(0).GetComponent<PathCreator>();
            transform.position = currentPath.path.GetPoint(0);
            NextPath();
        }

        private void Update()
        {
            if (DataManager.IsPointerOverGameObject == Switch.ON || Main.Instance.objSelectPen.gameObject.activeInHierarchy == true)
                return;

            SoundManager.Instance.PencilVolume(0f);

            if (moveEnd == true)
            {
                distanceTravelled += speed * Time.deltaTime / 1.5f;
                transform.rotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                transform.position = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);

                if (distanceTravelled >= currentPath.path.length)
                {
                    moveEnd = false;
                    NextPath();
                }
                return;
            }

            if (Input.GetMouseButtonDown(0) && isTouch == false && busy == false && Main.Instance.objColoring.gameObject.activeInHierarchy == false)
            {
                timeTouch = Time.time;
                //Debug.Log("OnMouseDown");
                SoundManager.Instance.Vibrate();
                isTouch = true;

                if (indexPath < lines.childCount)
                {
                    MarkXO.Instance.transform.GetChild(0).gameObject.SetActive(false);
                    MarkXO.Instance.transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    transform.GetChild(transform.childCount - 1).gameObject.SetActive(true);
                }

                PenRotate.Instance.transform.DOLocalMove(Vector3.zero, 0.1f);

                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.z = 0f;
                offset = target - transform.position;
            }

            if (Input.GetMouseButtonUp(0) && Main.Instance.objColoring.gameObject.activeInHierarchy == false)
            {
                //Debug.Log("OnMouseUp");
                isTouch = false;

                SoundManager.Instance.PencilVolume(0f);
                PenRotate.Instance.MouseUp();

                if (indexPath < lines.childCount)
                {
                    if (distanceTravelled > currentPath.path.length - 0.5f)
                    {
                        moveEnd = true;
                        //NextPath();
                        return;
                    }

                    if (distanceTravelled > currentPath.path.length)
                    {
                        NextPath(false);
                    }
                }
                else
                {
                    transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
                }
            }

            if (Input.GetMouseButton(0) && isTouch && Time.time - timeTouch > 0.15f)
            {
                if (autoBrush)
                {
                    target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    target.z = 0f;
                    target = target - offset;

                    transform.position = target;

                    //if (pathCreators.GetChild(indexPath).childCount > 1 ||
                    //    fills.GetChild(indexPath - lines.childCount).GetComponent<TriggerFillManager>().getCollidder2D().OverlapPoint(target))
                    //{
                    //    transform.position = target;
                    //}
                    //else
                    //{
                    //    target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //    target.z = 0f;
                    //    offset = target - transform.position;
                    //}

                    BoundScreen();

                    if (isMouseDrag)
                        SoundManager.Instance.PencilVolume(0.4f);
                    else
                        SoundManager.Instance.PencilVolume(0f);

                    return;
                }

                if (currentPath != null)
                {
                    SoundManager.Instance.PencilVolume(0.4f);

                    if (distanceTravelled < currentPath.path.length * 0.9f) distanceTravelled += speed * Time.deltaTime;
                    else distanceTravelled += speed * Time.deltaTime / 1.5f;

                    if (distanceTravelled > currentPath.path.length && isOutSide == false)
                    {
                        timeOutside = Time.time;
                        isOutSide = true;
                    }

                    if (isOutSide)
                    {
                        if (Time.time - timeOutside < 1f)
                        {
                            transform.position += transform.forward * Time.deltaTime * speed / 6f;
                            BoundScreen();
                        }
                        return;
                    }

                    transform.rotation = currentPath.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                    transform.position = currentPath.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                }
            }
        }

        public void NextPath(bool nice = true)
        {
            int total = lines.childCount + fills.childCount;

            PenRotate.Instance.MouseUp();

            if (indexPath + 1 < total)
            {
                transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
                distanceTravelled = 0f;

                if (ps != null)
                {
                    emissionModule = ps.emission;
                    emissionModule.rateOverDistance = 0f;
                }

                indexPath++;
                currentPath = pathCreators.GetChild(indexPath).GetComponent<PathCreator>();

                if (indexPath > 0 && indexPath <= lines.childCount) // cho hiện đường line sau khi tô xong.
                {
                    OnEndPath(nice);
                }
                else if (indexPath > lines.childCount && indexPath <= total) // cho hiện màu fill sau khi tô xong.
                {
                    if (nice)
                    {
                        fills.GetChild(indexPath - lines.childCount - 1).GetComponent<SpriteMask>().enabled = false;
                        //Mau da duoc chon
                        fills.GetChild(indexPath - lines.childCount - 1).GetComponent<SpriteRenderer>().color = selectColor;
                    }

                    if (ps != null)
                        ps.Clear();

                    OnEndPath(nice);
                }

                if (indexPath > -1 && indexPath < lines.childCount) // cho hiện mask khi chuyển phần tử line.
                {
                    currentPath.gameObject.SetActive(true);
                    isOutSide = false;
                    busy = true;

                    transform.DOMove(currentPath.path.GetPoint(0), 0.25f).OnComplete(() =>
                    {
                        ps = Instantiate(DataManager.GetBrushBlack(), transform.position, Quaternion.identity, transform);
                        ps.GetComponent<ParticleSystemRenderer>().sortingLayerName = lines.GetChild(indexPath).GetComponent<SpriteRenderer>().sortingLayerName;
                        ps.GetComponent<ParticleSystemRenderer>().sortingOrder = lines.GetChild(indexPath).GetComponent<SpriteRenderer>().sortingOrder;
                        ps.transform.SetSiblingIndex(0);

                        busy = false;
                    });

                    Vector3 cameraFollow = currentPath.transform.GetChild(0).position;
                    cameraFollow.z = -10f;
                    Camera.main.transform.DOMove(cameraFollow, 0.25f);

                    MarkXO.Instance.transform.GetChild(0).position = currentPath.path.GetPoint(0);
                    MarkXO.Instance.transform.GetChild(0).gameObject.SetActive(true);
                    MarkXO.Instance.transform.GetChild(1).position = currentPath.path.GetPoint(currentPath.path.localPoints.Length - 1);
                }
                else if (indexPath > lines.childCount - 1 && indexPath < total) // cho hiện mask khi chuyển phần tử fill.
                {
                    autoBrush = true;

                    currentPath.transform.GetChild(0).gameObject.SetActive(true);
                    currentPath.gameObject.SetActive(true);
                    effectStrip = StartCoroutine(EffectStrip(currentPath.transform.GetChild(0).GetComponent<SpriteRenderer>()));

                    transform.DOMove(new Vector3(10f, -10f, 0f), 0.5f);

                    if (indexPath == lines.childCount && oneBonus == 0)
                    {
                        oneBonus = 1;
                        Vector3 cameraFollow = Vector3.zero;
                        cameraFollow.z = -10f;

                        Camera.main.transform.DOMove(cameraFollow, 0.25f);

                        GameObject a = new GameObject();
                        a.AddComponent<SpriteRenderer>().sprite = DataManager.GetSpriteDone();
                        a.transform.SetParent(transform.parent);
                        a.transform.localScale = Vector3.one;
                        a.GetComponent<SpriteRenderer>().sortingLayerName = "Pen";
                    }

                    int randomIndex = Random.Range(0, 3);
                    //Chon Mau Chinh
                    myColors[randomIndex] = defaultColor[indexPath - lines.childCount];
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == randomIndex)
                            continue;

                        if (i > 0)
                        {
                            myColors[i] = RandomColor(myColors[i - 1]);
                            continue;
                        }

                        myColors[i] = RandomColor(Color.white);
                    }

                    SoundManager.Instance.PencilVolume(0f);
                    Main.Instance.objColoring.WakeUp(myColors);

                    fills.GetChild(indexPath - lines.childCount).GetComponent<SpriteRenderer>().color = Color.white;
                    fills.GetChild(indexPath - lines.childCount).GetComponent<SpriteRenderer>().enabled = true;
                    fills.GetChild(indexPath - lines.childCount).GetComponent<SpriteMask>().enabled = true;
                    fills.GetChild(indexPath - lines.childCount).gameObject.SetActive(true);
                }
                return;
            }

            if (isDone) return;

            isDone = true;

            fills.GetChild(fills.childCount - 1).GetComponent<SpriteMask>().enabled = false;
            fills.GetChild(fills.childCount - 1).GetComponent<SpriteRenderer>().color = selectColor;

            OnEndPath(true);

            Main.Instance.objCompleted.gameObject.SetActive(true);
            Main.Instance.psConfetti.Play();

            transform.DOMove(new Vector3(10f, -10f, 0f), 0.5f);
            Camera.main.DOOrthoSize(Main.Instance.defaultCamera * 1.25f, 0.5f);
            Camera.main.transform.DOScale(Main.Instance.defaultCamera / 5f, 0.5f);
        }

        IEnumerator DelayFrame(int frame, System.Action callback)
        {
            for (int i = 0; i < frame; i++)
            {
                yield return null;
            }

            callback.Invoke();
        }

        public void Reload()
        {
            if (isDone || canReload == false)
                return;

            StartCoroutine(DelayReload());
            PenRotate.Instance.MouseUp();

            if (indexPath > -1 && indexPath < lines.childCount) // reload line
            {
                if (distanceTravelled > 0)
                {
                    Debug.Log("Distance travelled > 0");
                    distanceTravelled = 0f;
                    emissionModule = ps.emission;
                    emissionModule.rateOverDistance = 0f;

                    MarkXO.Instance.transform.GetChild(0).gameObject.SetActive(true);
                    MarkXO.Instance.transform.GetChild(1).gameObject.SetActive(false);

                    busy = true;
                    transform.DOMove(currentPath.path.GetPoint(0), 0.05f).OnComplete(() =>
                    {
                        ps.Clear();
                        busy = false;
                        StartCoroutine(DelayFrame(1, () => emissionModule.rateOverDistance = 50f));
                    });
                    return;
                }

                if (indexPath > 0)
                {
                    Debug.Log("Path > 0");
                    Destroy(transform.GetChild(0).gameObject);
                    Destroy(transform.GetChild(1).gameObject);
                    pathCreators.GetChild(indexPath).gameObject.SetActive(false);

                    MarkXO.Instance.transform.GetChild(0).gameObject.SetActive(true);
                    MarkXO.Instance.transform.GetChild(1).gameObject.SetActive(false);

                    indexPath = indexPath - 2;
                    NextPath(false);
                    return;
                }

                Debug.Log("Path == 0");
                Destroy(transform.GetChild(0).gameObject);
                pathCreators.GetChild(indexPath).gameObject.SetActive(false);

                MarkXO.Instance.transform.GetChild(0).gameObject.SetActive(false);
                MarkXO.Instance.transform.GetChild(1).gameObject.SetActive(false);

                indexPath--;
                NextPath();
                return;
            }

            // reload fill

            if (indexPath == lines.childCount) // to mau dau tien
            {
                if (Main.Instance.objColoring.gameObject.activeInHierarchy == false) // chon mau roi
                {
                    //if (fills.GetChild(indexPath - lines.childCount).GetComponent<TriggerFillManager>().CountTrigger > 0) // to mau roi
                    //{
                    Debug.Log("To mau dau tien -> chon mau roi -> to mau roi");
                    transform.DOMove(new Vector3(10f, -10f, 0f), 0.5f);
                    transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);

                    Destroy(transform.GetChild(0).gameObject);

                    currentPath.transform.GetChild(0).gameObject.SetActive(true);
                    effectStrip = StartCoroutine(EffectStrip(currentPath.transform.GetChild(0).GetComponent<SpriteRenderer>()));

                    fills.GetChild(indexPath - lines.childCount).GetComponent<TriggerFillManager>().Reload();

                    int randomIndex = Random.Range(0, 3);
                    //Chon Mau chinh
                    myColors[randomIndex] = defaultColor[indexPath - lines.childCount];

                    for (int i = 0; i < 3; i++)
                    {
                        if (i == randomIndex)
                            continue;

                        if (i > 0)
                        {
                            myColors[i] = RandomColor(myColors[i - 1]);
                            continue;
                        }

                        myColors[i] = RandomColor(Color.white);
                    }

                    Main.Instance.objColoring.WakeUp(myColors);
                    Main.Instance.btnDone.gameObject.SetActive(false);
                    //}
                    //else // chua to mau
                    //{
                    //    Debug.Log("To mau dau tien -> chon mau roi -> chua to mau");
                    //    autoBrush = false;
                    //    PenRotate.Instance.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black; // doi but sang mau den
                    //    Destroy(transform.parent.GetChild(transform.parent.childCount - 1).gameObject); // xoa duong line bonus
                    //    oneBonus = 0;

                    //    Destroy(transform.GetChild(0).gameObject); // xoa fill hien tai
                    //    Destroy(transform.GetChild(1).gameObject); // xoa line truoc do

                    //    currentPath.gameObject.SetActive(false); // an path hien tai
                    //    fills.GetChild(indexPath - lines.childCount).GetComponent<TriggerFillManager>().Reload();
                    //    fills.GetChild(indexPath - lines.childCount).gameObject.SetActive(false);

                    //    distanceTravelled = 0f;
                    //    MarkXO.Instance.transform.GetChild(0).gameObject.SetActive(true);
                    //    MarkXO.Instance.transform.GetChild(1).gameObject.SetActive(false);

                    //    indexPath -= 2;
                    //    NextPath(false);
                    //}

                    return;

                }

                // chua chon mau

                Debug.Log("To mau dau tien -> chua chon mau");

                autoBrush = false;
                PenRotate.Instance.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black; // doi but sang mau den
                Destroy(transform.parent.GetChild(transform.parent.childCount - 1).gameObject); // xoa duong line bonus
                oneBonus = 0;

                Destroy(transform.GetChild(0).gameObject);

                Main.Instance.objColoring.gameObject.SetActive(false);

                currentPath.gameObject.SetActive(false);
                fills.GetChild(indexPath - lines.childCount).GetComponent<TriggerFillManager>().Reload();
                fills.GetChild(indexPath - lines.childCount).gameObject.SetActive(false);

                distanceTravelled = 0f;
                MarkXO.Instance.transform.GetChild(0).gameObject.SetActive(true);
                MarkXO.Instance.transform.GetChild(1).gameObject.SetActive(false);

                indexPath -= 2;
                NextPath(false);
                return;
            }

            // to mau sau do
            if (Main.Instance.objColoring.gameObject.activeInHierarchy == false) // chon mau roi
            {
                if (fills.GetChild(indexPath - lines.childCount).GetComponent<TriggerFillManager>().CountTrigger > 0) // to mau roi
                {
                    Debug.Log("mau sau do -> chon mau roi -> to mau roi");
                    transform.DOMove(new Vector3(10f, -10f, 0f), 0.5f);
                    Destroy(transform.GetChild(0).gameObject);

                    currentPath.transform.GetChild(0).gameObject.SetActive(true);
                    effectStrip = StartCoroutine(EffectStrip(currentPath.transform.GetChild(0).GetComponent<SpriteRenderer>()));

                    fills.GetChild(indexPath - lines.childCount).GetComponent<TriggerFillManager>().Reload();

                    int randomIndex = Random.Range(0, 3);
                    //Chon Mau Chinh
                    myColors[randomIndex] = defaultColor[indexPath - lines.childCount];

                    for (int i = 0; i < 3; i++)
                    {
                        if (i == randomIndex)
                            continue;

                        if (i > 0)
                        {
                            myColors[i] = RandomColor(myColors[i - 1]);
                            continue;
                        }

                        myColors[i] = RandomColor(Color.white);
                    }

                    Main.Instance.objColoring.WakeUp(myColors);
                    Main.Instance.btnDone.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("mau sau do -> chon mau roi -> chua to mau");
                    transform.DOMove(new Vector3(10f, -10f, 0f), 0.5f);
                    Destroy(transform.GetChild(0).gameObject);
                    Destroy(transform.GetChild(1).gameObject);

                    currentPath.gameObject.SetActive(false);
                    fills.GetChild(indexPath - lines.childCount).GetComponent<TriggerFillManager>().Reload();
                    fills.GetChild(indexPath - lines.childCount).gameObject.SetActive(false);

                    fills.GetChild(indexPath - lines.childCount - 1).GetComponent<TriggerFillManager>().Reload();
                    fills.GetChild(indexPath - lines.childCount - 1).gameObject.SetActive(false);

                    indexPath -= 2;
                    NextPath(false);
                }
                return;
            }

            // chua chon mau
            Debug.Log("mau sau do -> chua chon mau");
            transform.DOMove(new Vector3(10f, -10f, 0f), 0.5f);
            Destroy(transform.GetChild(0).gameObject);

            currentPath.gameObject.SetActive(false);
            fills.GetChild(indexPath - lines.childCount).GetComponent<TriggerFillManager>().Reload();
            fills.GetChild(indexPath - lines.childCount).gameObject.SetActive(false);

            fills.GetChild(indexPath - lines.childCount - 1).GetComponent<TriggerFillManager>().Reload();
            fills.GetChild(indexPath - lines.childCount - 1).gameObject.SetActive(false);

            indexPath -= 2;
            NextPath(false);
        }

        public void SelectColor(Color color)
        {   
            //Mau da duoc chon
            selectColor = color;

            PenRotate.Instance.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;

            pathCreators.GetChild(indexPath).GetChild(0).gameObject.SetActive(false);

            StopCoroutine(effectStrip);

            busy = true;
            transform.DOMove(currentPath.path.GetPoint(0), 0.25f).OnComplete(() =>
            {
                ps = Instantiate(DataManager.GetBrushColor(), transform.position, Quaternion.identity, transform);
                ps.transform.SetSiblingIndex(0);

                ps.GetComponent<ParticleSystemRenderer>().sortingLayerName = fills.GetChild(indexPath - lines.childCount).GetComponent<SpriteRenderer>().sortingLayerName;
                ps.GetComponent<ParticleSystemRenderer>().sortingOrder = fills.GetChild(indexPath - lines.childCount).GetComponent<SpriteRenderer>().sortingOrder + 1;

                ParticleSystem.MainModule mainPs = ps.main;
                mainPs.startColor = color;

                busy = false;
            });
        }

        void OnEndPath(bool nice)
        {
            isTouch = false;

            SoundManager.Instance.PlayEffect(SoundManager.Instance.clipSuccess);
            SoundManager.Instance.Vibrate();

            if (nice)
                SpawnFX.Instance.Show(transform.position);

            MarkXO.Instance.transform.GetChild(0).gameObject.SetActive(false);
            MarkXO.Instance.transform.GetChild(1).gameObject.SetActive(false);
        }
        //RanDom Chon Mau
        Color RandomColor(Color a)
        {
            Color temp = Color.white;
            float d = 0f;

            while (d < 1f)
            {
                temp.r = Random.Range(0f, 1f);
                temp.g = Random.Range(0f, 1f);
                temp.b = Random.Range(0f, 1f);

                d = Mathf.Sqrt(Mathf.Pow(temp.r - a.r, 2) + Mathf.Pow(temp.g - a.g, 2) + Mathf.Pow(temp.b - a.b, 2));
            }

            return temp;
        }

        IEnumerator EffectStrip(SpriteRenderer obj)
        {
            float speed = 1f;

            obj.sortingLayerName = fills.GetChild(indexPath - lines.childCount).GetComponent<SpriteRenderer>().sortingLayerName;
            obj.sortingOrder = fills.GetChild(indexPath - lines.childCount).GetComponent<SpriteRenderer>().sortingOrder + 1;

            Color color = obj.color;
            while (true)
            {
                yield return null;
                color.a += Time.deltaTime * speed;
                obj.color = color;

                if (color.a >= 1f)
                {
                    speed = -1f;
                }

                if (color.a <= 0.3f)
                {
                    speed = 1f;
                }
            }
        }

        IEnumerator DelayReload()
        {
            canReload = false;
            yield return new WaitForSeconds(0.5f);
            canReload = true;
        }

        void BoundScreen()
        {
            float h = Camera.main.orthographicSize * 0.8f;
            float w = h * Camera.main.aspect;

            float maxH = Camera.main.transform.position.y + h;
            float minH = Camera.main.transform.position.y - h;
            float maxW = Camera.main.transform.position.x + w;
            float minW = Camera.main.transform.position.x - w;

            if (transform.position.y > maxH)
            {
                transform.position = new Vector3(transform.position.x, maxH);
            }

            if (transform.position.y < minH)
            {
                transform.position = new Vector3(transform.position.x, minH);
            }

            if (transform.position.x > maxW)
            {
                transform.position = new Vector3(maxW, transform.position.y);
            }

            if (transform.position.x < minW)
            {
                transform.position = new Vector3(minW, transform.position.y);
            }
        }

        IEnumerator IsMouseDrag()
        {
            Vector3 pre;
            while (true)
            {
                pre = transform.position;

                yield return new WaitForSeconds(0.1f);

                if (pre != transform.position)
                    isMouseDrag = true;
                else
                    isMouseDrag = false;
            }
        }
    }
}
