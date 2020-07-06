using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views.UI
{
    public class MessageWindow : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private Text _messageField;
        [SerializeField] private Button _apply;
        [SerializeField] private Button _cancel;
#pragma warning restore 0649

        public async Task<bool> Show(string message)
        {
            _messageField.text = message;
            
            var tcs = new TaskCompletionSource<bool>();

            void ApplyHandler() => tcs.SetResult(true);
            void CancelHandler() => tcs.SetResult(false);

            _apply.onClick.AddListener(ApplyHandler);
            _cancel.onClick.AddListener(CancelHandler);

            tcs.Task.ContinueWith(_ =>
            {
                _apply.onClick.RemoveListener(ApplyHandler);
                _cancel.onClick.RemoveListener(CancelHandler);
                Destroy(gameObject);
            }, TaskScheduler.FromCurrentSynchronizationContext());

            return await tcs.Task;
        }
    }
}