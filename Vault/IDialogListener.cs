namespace Vault
{
    public interface IDialogListener
    {
        void OnDialogAction(DialogAction action, string actionType = "");
    }

    public enum DialogAction
    {
        ACTION,
        CANCEL
    }
}
