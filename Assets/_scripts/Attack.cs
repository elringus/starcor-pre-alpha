
public class Attack 
{
	public FOF Fof { get; set; }
	public float Damage { get; set; }

	public Attack (float damage, FOF fof)
	{
		this.Damage = damage;
		this.Fof = fof;
	}
}