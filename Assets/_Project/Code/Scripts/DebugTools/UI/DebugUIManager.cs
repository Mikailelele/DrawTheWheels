using DrawCar.Ui;
using DrawCar.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace DrawCar.DebugTools.Ui
{
    public sealed class DebugUIManager : MonoBehaviour
    {
        [SerializeField] private BaseButtonController _clearSavesButton;
        [SerializeField] private int _maxClicksToResetSave = 5;
        
        private int _counterToResetSave;

        private void Start()
        {
            _clearSavesButton.OnPointerUpAction += OnClearButtonClicked;
            _clearSavesButton.OnPointerExitAction += OnClearButtonExit;
        }

        private void OnDestroy()
        {
            _clearSavesButton.OnPointerUpAction -= OnClearButtonClicked;
            _clearSavesButton.OnPointerExitAction -= OnClearButtonExit;
        }

        private void OnClearButtonClicked()
        {
            _counterToResetSave++;
            if (_counterToResetSave >= _maxClicksToResetSave)
            {
                DebugService.ClearSave();

                ResetSaveCounter();
                SceneManager.LoadScene(YandexGame.savesData.LevelNumber);
                this.Log("Saves cleared");
            }
        }

        private void OnClearButtonExit()
        {
            ResetSaveCounter();
        }

        private void ResetSaveCounter()
        {
            _counterToResetSave = 0;
        }
    }
}