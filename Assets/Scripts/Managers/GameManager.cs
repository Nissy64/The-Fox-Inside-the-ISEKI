using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public Image silhouette;
        [Range(0, 10)]
        public float startSilhouetteScalingSeconds = 1f;
        [Range(0, 10)]
        public float endSilhouetteScalingSeconds = 1f;
        public float startWaitSec = 1;
        public Ease startSilhouetteEase = Ease.InOutCirc;
        public Ease endSilhouetteEase = Ease.InOutCubic;
        private int defaultsilhouetteScale = 5000;
        [ReadOnly]
        public bool isGameOver;

        void Awake()
        {
            isGameOver = false;
        }

        void Start()
        {
            Invoke(nameof(StartGame), startWaitSec);
        }

        void Update()
        {
            if(isGameOver)
            {
                GameOver();
            }
        }

        private void StartGame()
        {
            silhouette.rectTransform.DOScale(Vector3.one * defaultsilhouetteScale, startSilhouetteScalingSeconds).SetEase(startSilhouetteEase);
        }

        private void GameOver()
        {
            silhouette.rectTransform.DOScale(Vector3.zero, endSilhouetteScalingSeconds).SetEase(endSilhouetteEase).OnComplete(() => Invoke(nameof(ReLoadScene), 0.5f));
        }

        private void ReLoadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}