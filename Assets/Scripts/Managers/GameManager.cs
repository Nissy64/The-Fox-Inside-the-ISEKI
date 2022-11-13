using System.Collections;
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
        public float silhouetteScalingSpeed = 0.25f;
        public Ease endSilhouetteEase = Ease.InOutCubic;
        public Ease startSilhouetteEase = Ease.InOutCirc;
        private int defaultsilhouetteScale = 15000;
        [ReadOnly]
        public bool isGameOver;

        void Awake()
        {
            isGameOver = false;
        }

        void Start()
        {
            Invoke(nameof(StartGame), 0.5f);
        }

        void Update()
        {
            if(isGameOver)
            {
                StartCoroutine(GameOver());
            }
        }

        private IEnumerator GameOver()
        {
            silhouette.rectTransform.DOScale(Vector3.zero, silhouetteScalingSpeed).SetEase(endSilhouetteEase);

            yield return new WaitForSeconds(silhouetteScalingSpeed);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void StartGame()
        {
            silhouette.rectTransform.DOScale(Vector3.one * defaultsilhouetteScale, silhouetteScalingSpeed).SetEase(startSilhouetteEase);
        }
    }
}