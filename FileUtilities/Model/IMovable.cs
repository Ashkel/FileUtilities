namespace FileUtilities.Model
{
	interface IMovable
	{
		void Move(ContainerWrapper newParent);
		void Remove();
	}
}
