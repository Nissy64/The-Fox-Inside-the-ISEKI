using UnityEngine;
using DG.Tweening;
using System.Collections;
using Managers;

namespace Objects
{
    public class VerticalMovePlatform : MonoBehaviour
    {
        public Transform mpTransform;
        public Transform playerTransform;
        public Rigidbody2D mpRb;
        public Rigidbody2D playerRb;
        public Animator mpAnimator;
        public string playerTag;
        [ReadOnly] 
        public float startMpPosition = 0;
        public float endMpPosition;
        public FolderManager.GizmosFiles startMpPositionIcon;
        public FolderManager.GizmosFiles endMpPositionIcon;
        public float mpDuration = 2;
        public int mpWaitSecond = 1;
        [Range(1, 100)] 
        public float downdingForceMultiply = 50;
        private Vector3 prevPosition;
        private Vector3 mpVelocity;
        private bool playerOnMp = false;

        void Awake()
        {
            startMpPosition = mpTransform.position.y;
            prevPosition = mpTransform.position;
        }

        void Start()
        {
            StartCoroutine(MoveStart());
        }

        void Update()
        {
            MpVelocity();

            if(playerOnMp)
            {
                playerRb.AddForce(Vector2.down * Mathf.Abs(mpVelocity.y )* downdingForceMultiply * Time.deltaTime);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawIcon(mpTransform.position, FolderManager.GetGizmosFiles(startMpPositionIcon), true);
            Gizmos.DrawIcon(new Vector3(mpTransform.position.x, endMpPosition, 0), FolderManager.GetGizmosFiles(endMpPositionIcon), true);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if(!collider.CompareTag(playerTag)) return;

            playerOnMp = true;
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if(!collider.CompareTag(playerTag)) return;
            playerRb.position = new Vector2(playerTransform.position.x, mpTransform.position.y + 1.4375f);
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if(!collider.CompareTag(playerTag)) return;

            playerOnMp = false;
        }

        private IEnumerator MoveStart()
        {
            mpAnimator.SetBool("UpToDown", true);

            yield return new WaitForSeconds(mpWaitSecond);

            mpAnimator.SetBool("UpToDown", false);

            mpRb.DOMove(new Vector2(mpTransform.position.x, endMpPosition), mpDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    StartCoroutine(MoveEnd());
                });
        }

        private IEnumerator MoveEnd()
        {
            mpAnimator.SetBool("DownToUp", true);

            yield return new WaitForSeconds(mpWaitSecond);

            mpAnimator.SetBool("DownToUp", false);

            mpRb.DOMove(new Vector2(mpTransform.position.x, startMpPosition), mpDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    StartCoroutine(MoveStart());
                });
        }

        private void MpVelocity()
        {
            if(Mathf.Approximately(Time.deltaTime, 0)) return;

            var position = mpTransform.position;

            mpVelocity = (position - prevPosition) / Time.deltaTime;

            prevPosition = position;
        }
    }
}