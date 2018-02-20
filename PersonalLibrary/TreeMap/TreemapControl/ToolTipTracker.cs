using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
namespace Microsoft.Research.CommunityTechnologies.GraphicsLib
{
	internal class ToolTipTracker : IDisposable
	{
		public delegate void ToolTipTrackerEvent(object oSource, ToolTipTrackerEventArgs oToolTipTrackerEventArgs);
		protected enum State
		{
			NotDoingAnything,
			WaitingForShowTimeout,
			WaitingForHideTimeout,
			WaitingForReshowTimeout
		}
		public const int MinDelayMs = 1;
		public const int MaxDelayMs = 10000;
		protected const int DefaultShowDelayMs = 500;
		protected const int DefaultHideDelayMs = 5000;
		protected const int DefaultReshowDelayMs = 50;
		protected int m_iShowDelayMs;
		protected int m_iHideDelayMs;
		protected int m_iReshowDelayMs;
		protected ToolTipTracker.State m_iState;
		protected object m_oObjectBeingTracked;
		protected Timer m_oTimer;
		protected bool m_bDisposed;
        public ToolTipTrackerEvent ShowToolTip;
        //public event ToolTipTracker.ToolTipTrackerEvent ShowToolTip
        //{
        //    [MethodImpl(32)]
        //    add
        //    {
        //        this.ShowToolTip = (ToolTipTracker.ToolTipTrackerEvent)Delegate.Combine(this.ShowToolTip, value);
        //    }
        //    [MethodImpl(32)]
        //    remove
        //    {
        //        this.ShowToolTip = (ToolTipTracker.ToolTipTrackerEvent)Delegate.Remove(this.ShowToolTip, value);
        //    }
        //}
        public event ToolTipTracker.ToolTipTrackerEvent HideToolTip;
        //public event ToolTipTracker.ToolTipTrackerEvent HideToolTip
        //{
        //    [MethodImpl(32)]
        //    add
        //    {
        //        this.HideToolTip = (ToolTipTracker.ToolTipTrackerEvent)Delegate.Combine(this.HideToolTip, value);
        //    }
        //    [MethodImpl(32)]
        //    remove
        //    {
        //        this.HideToolTip = (ToolTipTracker.ToolTipTrackerEvent)Delegate.Remove(this.HideToolTip, value);
        //    }
        //}
		public int ShowDelayMs
		{
			get
			{
				this.AssertValid();
				return this.m_iShowDelayMs;
			}
			set
			{
				this.ValidateDelayProperty(value, "ShowDelayMs");
				this.m_iShowDelayMs = value;
			}
		}
		public int HideDelayMs
		{
			get
			{
				this.AssertValid();
				return this.m_iHideDelayMs;
			}
			set
			{
				this.ValidateDelayProperty(value, "HideDelayMs");
				this.m_iHideDelayMs = value;
			}
		}
		public int ReshowDelayMs
		{
			get
			{
				this.AssertValid();
				return this.m_iReshowDelayMs;
			}
			set
			{
				this.ValidateDelayProperty(value, "ReshowDelayMs");
				this.m_iReshowDelayMs = value;
			}
		}
		public ToolTipTracker()
		{
			this.m_iShowDelayMs = 500;
			this.m_iHideDelayMs = 5000;
			this.m_iReshowDelayMs = 50;
			this.m_iState = ToolTipTracker.State.NotDoingAnything;
			this.m_oObjectBeingTracked = null;
			this.m_bDisposed = false;
			this.m_oTimer = new Timer();
			this.m_oTimer.Tick += new EventHandler(this.TimerTick);
		}
		~ToolTipTracker()
		{
			this.Dispose(false);
		}
		public void OnMouseMoveOverObject(object oObjectToTrack)
		{
			this.AssertValid();
			switch (this.m_iState)
			{
				case ToolTipTracker.State.NotDoingAnything:
				{
					if (oObjectToTrack != null)
					{
						this.ChangeState(ToolTipTracker.State.WaitingForShowTimeout, oObjectToTrack);
					}
					break;
				}
				case ToolTipTracker.State.WaitingForShowTimeout:
				{
					if (oObjectToTrack == null)
					{
						this.ChangeState(ToolTipTracker.State.NotDoingAnything, null);
					}
					else
					{
						if (oObjectToTrack != this.m_oObjectBeingTracked)
						{
							this.ChangeState(ToolTipTracker.State.WaitingForShowTimeout, oObjectToTrack);
						}
					}
					break;
				}
				case ToolTipTracker.State.WaitingForHideTimeout:
				{
					if (oObjectToTrack == null)
					{
						this.FireHideToolTipEvent(this.m_oObjectBeingTracked);
						this.ChangeState(ToolTipTracker.State.WaitingForReshowTimeout, null);
					}
					else
					{
						if (oObjectToTrack == this.m_oObjectBeingTracked)
						{
							this.ChangeState(ToolTipTracker.State.WaitingForHideTimeout, oObjectToTrack);
						}
						else
						{
							this.FireHideToolTipEvent(this.m_oObjectBeingTracked);
							this.FireShowToolTipEvent(oObjectToTrack);
							this.ChangeState(ToolTipTracker.State.WaitingForHideTimeout, oObjectToTrack);
						}
					}
					break;
				}
				case ToolTipTracker.State.WaitingForReshowTimeout:
				{
					if (oObjectToTrack != null)
					{
						this.FireShowToolTipEvent(oObjectToTrack);
						this.ChangeState(ToolTipTracker.State.WaitingForHideTimeout, oObjectToTrack);
					}
					break;
				}
				default:
				{
					Debug.Assert(false);
					break;
				}
			}
		}
		public void Reset()
		{
			this.AssertValid();
			this.m_oTimer.Stop();
			if (this.m_iState == ToolTipTracker.State.WaitingForHideTimeout)
			{
				this.FireHideToolTipEvent(this.m_oObjectBeingTracked);
			}
			this.ChangeState(ToolTipTracker.State.NotDoingAnything, null);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected void ValidateDelayProperty(int iValue, string sPropertyName)
		{
			if (iValue < 1 || iValue > 10000)
			{
				throw new ArgumentOutOfRangeException(sPropertyName, iValue, string.Concat(new object[]
				{
					"ToolTipTracker.", 
					sPropertyName, 
					": Must be between ", 
					1, 
					" and ", 
					10000, 
					"."
				}));
			}
		}
		protected void ChangeState(ToolTipTracker.State iState, object oObjectToTrack)
		{
			this.AssertValid();
			this.m_oTimer.Stop();
			this.m_iState = iState;
			this.m_oObjectBeingTracked = oObjectToTrack;
			int num;
			switch (iState)
			{
				case ToolTipTracker.State.NotDoingAnything:
				{
					num = -1;
					break;
				}
				case ToolTipTracker.State.WaitingForShowTimeout:
				{
					num = this.m_iShowDelayMs;
					break;
				}
				case ToolTipTracker.State.WaitingForHideTimeout:
				{
					num = this.m_iHideDelayMs;
					break;
				}
				case ToolTipTracker.State.WaitingForReshowTimeout:
				{
					num = this.m_iReshowDelayMs;
					break;
				}
				default:
				{
					Debug.Assert(false);
					num = -1;
					break;
				}
			}
			if (num != -1)
			{
				this.m_oTimer.Interval = num;
				this.m_oTimer.Start();
			}
			this.AssertValid();
		}
		protected void FireShowToolTipEvent(object oObject)
		{
			Debug.Assert(oObject != null);
			if (this.ShowToolTip != null)
			{
				this.ShowToolTip(this, new ToolTipTrackerEventArgs(oObject));
			}
		}
		protected void FireHideToolTipEvent(object oObject)
		{
			Debug.Assert(oObject != null);
			if (this.HideToolTip != null)
			{
				this.HideToolTip(this, new ToolTipTrackerEventArgs(oObject));
			}
		}
		protected void TimerTick(object oSource, EventArgs oEventArgs)
		{
			this.AssertValid();
			this.m_oTimer.Stop();
			switch (this.m_iState)
			{
				case ToolTipTracker.State.NotDoingAnything:
				{
					Debug.Assert(false);
					break;
				}
				case ToolTipTracker.State.WaitingForShowTimeout:
				{
					this.FireShowToolTipEvent(this.m_oObjectBeingTracked);
					this.ChangeState(ToolTipTracker.State.WaitingForHideTimeout, this.m_oObjectBeingTracked);
					break;
				}
				case ToolTipTracker.State.WaitingForHideTimeout:
				{
					this.FireHideToolTipEvent(this.m_oObjectBeingTracked);
					this.ChangeState(ToolTipTracker.State.WaitingForReshowTimeout, null);
					break;
				}
				case ToolTipTracker.State.WaitingForReshowTimeout:
				{
					this.ChangeState(ToolTipTracker.State.NotDoingAnything, null);
					break;
				}
				default:
				{
					Debug.Assert(false);
					break;
				}
			}
		}
		protected void Dispose(bool bDisposing)
		{
			if (!this.m_bDisposed && bDisposing)
			{
				this.m_oTimer.Stop();
				this.m_oTimer.Dispose();
			}
			this.m_bDisposed = true;
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(this.m_iShowDelayMs >= 1);
			Debug.Assert(this.m_iShowDelayMs <= 10000);
			Debug.Assert(this.m_iHideDelayMs >= 1);
			Debug.Assert(this.m_iHideDelayMs <= 10000);
			Debug.Assert(this.m_iReshowDelayMs >= 1);
			Debug.Assert(this.m_iReshowDelayMs <= 10000);
			switch (this.m_iState)
			{
				case ToolTipTracker.State.NotDoingAnything:
				{
					Debug.Assert(this.m_oObjectBeingTracked == null);
					break;
				}
				case ToolTipTracker.State.WaitingForShowTimeout:
				{
					Debug.Assert(this.m_oObjectBeingTracked != null);
					break;
				}
				case ToolTipTracker.State.WaitingForHideTimeout:
				{
					Debug.Assert(this.m_oObjectBeingTracked != null);
					break;
				}
				case ToolTipTracker.State.WaitingForReshowTimeout:
				{
					Debug.Assert(this.m_oObjectBeingTracked == null);
					break;
				}
				default:
				{
					Debug.Assert(false);
					break;
				}
			}
		}
	}
}
