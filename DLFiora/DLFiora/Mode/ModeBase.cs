namespace DLFiora.Controller
{
    public abstract class ModeBase
    {
        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
