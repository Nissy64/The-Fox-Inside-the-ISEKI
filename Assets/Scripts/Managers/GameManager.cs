using System.Collections;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public Image silhouette;
        [Range(0, 2)]
        public float startSilhouetteScalingSeconds = 1f;
        [Range(0, 2)]
        public float endSilhouetteScalingSeconds = 1f;
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
            Invoke(nameof(StartGame), 0.25f);
        }

        void Update()
        {
            if(isGameOver)
            {
                StartCoroutine(GameOver());
            }
        }

        private void StartGame()
        {
            silhouette.rectTransform.DOScale(Vector3.one * defaultsilhouetteScale, startSilhouetteScalingSeconds).SetEase(startSilhouetteEase);
        }

        private IEnumerator GameOver()
        {
            silhouette.rectTransform.DOScale(Vector3.zero, endSilhouetteScalingSeconds).SetEase(endSilhouetteEase);

            yield return new WaitForSeconds(endSilhouetteScalingSeconds);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}