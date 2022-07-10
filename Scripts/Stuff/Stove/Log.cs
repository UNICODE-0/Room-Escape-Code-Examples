public class Log : UseableItem
{
    override protected void OnTargetPosition()
    {
        base.OnTargetPosition();
        EnableColliderWithoutPhysic();
    }
}
