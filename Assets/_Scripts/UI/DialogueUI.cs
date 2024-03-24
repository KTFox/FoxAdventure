using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RPG.Dialogues;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private TextMeshProUGUI _AIText;
        [SerializeField]
        private GameObject _AIResponse;
        [SerializeField]
        private Button _nextButton;
        [SerializeField]
        private Button _quitButton;
        [SerializeField]
        private Transform _choiceRoot;
        [SerializeField]
        private GameObject _choiceButtonPrefab;

        private PlayerConversation _playerConversant;


        // Methods

        private void Start()
        {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversation>();

            _playerConversant.OnConversationUpdated += UpdateUI;
            _nextButton.onClick.AddListener(() =>
            {
                _playerConversant.MoveToNextDialogueNode();
            });
            _quitButton.onClick.AddListener(() =>
            {
                _playerConversant.QuitDialogue();
            });

            UpdateUI();
        }

        private void UpdateUI()
        {
            gameObject.SetActive(_playerConversant.IsActiveDialogue());

            if (!_playerConversant.IsActiveDialogue()) return;

            _choiceRoot.gameObject.SetActive(_playerConversant.IsChoosing());
            _AIResponse.SetActive(!_playerConversant.IsChoosing());

            if (_playerConversant.IsChoosing())
            {
                BuildChoicesList();
            }
            else
            {
                _AIText.text = _playerConversant.GetCurrentDialogueText();
                _nextButton.gameObject.SetActive(_playerConversant.HasNextDialogueNode());
            }
        }

        private void BuildChoicesList()
        {
            foreach (Transform child in _choiceRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (DialogueNodeSO choiceDialogue in _playerConversant.GetChoices())
            {
                GameObject choiceInstance = Instantiate(_choiceButtonPrefab, _choiceRoot);

                TextMeshProUGUI choiceText = choiceInstance.GetComponentInChildren<TextMeshProUGUI>();
                choiceText.text = choiceDialogue.GetText();

                Button choiceButton = choiceInstance.GetComponentInChildren<Button>();
                choiceButton.onClick.AddListener(() =>
                {
                    _playerConversant.SelectChoice(choiceDialogue);
                });
            }
        }
    }
}
