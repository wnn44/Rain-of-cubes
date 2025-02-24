using System;

public interface INotifier
{
    public event Action<INotifier> CubeEndedLife;    
}
