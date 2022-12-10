using UnityEngine;
using System.Collections;
using DG.Tweening;
using Player;
using Managers;

namespace Objects
{
    public class PressRock : MonoBehaviour
    {
        public Transform pressRockTransform;
        public BoxCollider2D armCollider2D;
        public FolderManager.GizmosFiles pressEndRockPositionIcon;
        [ReadOnly]
        public float startPressRockPosition;
        public float endPressRockPosition;
        public float startPressRockDuration = 2;
        public float endPressRockDuration = 2;
        public float startPressRockWaitSecond = 0.5f;
        public float pressedRockWaitSecond = 1;
        public Ease startPressRockEase = Ease.InQuad;
        public Ease endPressRockEase = Ease.OutQuad;
        public Vector2 pressAreaSize;
        public LayerMask playerLayer;
        [ReadOnly]
        public bool isPressing;
        [Range(8, 32)]
        public int pressRockHeight = 8;

        void Start()
        {
            startPressRockPosition = pressRockTransform.localPosition.y;
            Invoke(nameof(PressStart), startPressRockWaitSecond);
        }

        void Update()
        {
            Press();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawIcon(new Vector3(pressRockTransform.position.x, endPressRockPosition, pressRockTransform.position.z), FolderManager.GetGizmosFiles(pressEndRockPositionIcon), true);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(pressRockTransform.position, pressAreaSize);
        }

        private void Press()
        {
            PlayerMovement playerMovement;
            Collider2D hitCollider2d;

            if(hitCollider2d = Physics2D.OverlapBox(pressRockTransform.position, pressAreaSize, 0, playerLayer))
            {
                playerMovement = hitCollider2d.GetComponent<PlayerMovement>();

                if(isPressing)
                {
                    StartCoroutine(playerMovement.PlayerGameOver());
                }
            }
        }

        private void PressStart()
        {
            isPressing = true;
            pressRockTransform.DOLocalMoveY(endPressRockPosition, startPressRockDuration)
                .SetEase(startPressRockEase)
                .OnComplete(() =>
                {
                    StartCoroutine(PressEnd());
                });
        }

        private IEnumerator PressEnd()
        {
            isPressing = false;
            yield return new WaitForSeconds(pressedRockWaitSecond);

            pressRockTransform.DOLocalMoveY(startPressRockPosition, endPressRockDuration)
                .SetEase(endPressRockEase)
                .OnComplete(() =>
                {
                    PressStart();
                });
        }
    }
}