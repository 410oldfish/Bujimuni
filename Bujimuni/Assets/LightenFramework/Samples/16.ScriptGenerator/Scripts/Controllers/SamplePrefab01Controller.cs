
using Lighten;

public partial class SamplePrefab01Controller : XEntityController
{
    private void Start()
    {
    }
    
    private void Update()
    {
    }

    public void Show1()
    {
        this.View.A1GameObject.SetActive(true);
        this.View.B1GameObject.SetActive(false);
    }
    public void Show2()
    {
        this.View.A1GameObject.SetActive(false);
        this.View.B1GameObject.SetActive(true);
    }
}