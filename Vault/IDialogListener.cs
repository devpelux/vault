namespace Vault
{
    public interface IDialogListener
    {
        void OnDialogAbort();
        void OnDialogAction(DialogAction action);
    }

    public enum DialogAction
    {
        OK,
        CANCEL,
        DELETE,
        EDIT
    }
}
