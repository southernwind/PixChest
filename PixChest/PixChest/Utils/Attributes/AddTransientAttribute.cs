namespace PixChest.Utils.Attributes;

public class AddTransientAttribute(Type? serviceType = null): Attribute {
	public Type? ServiceType {
		get;
	} = serviceType;
}
