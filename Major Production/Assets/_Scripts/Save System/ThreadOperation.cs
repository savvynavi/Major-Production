using System.Threading;

namespace RPG.Save
{
	public class ThreadOperation
	{
		protected Thread thread;
		public bool IsDone { get { return !thread.IsAlive; } }
	}
}
