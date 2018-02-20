using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
namespace Microsoft.Research.CommunityTechnologies.AppLib
{
	public class HistoryList
	{
		public delegate void ChangeEventHandler(object oSource, EventArgs oEventArgs);
		protected ArrayList m_oStateList;
		protected int m_iCurrentObjectIndex;
        public event HistoryList.ChangeEventHandler Change;
        //public event HistoryList.ChangeEventHandler Change
        //{
        //    [MethodImpl(32)]
        //    add
        //    {
        //        this.Change = (HistoryList.ChangeEventHandler)Delegate.Combine(this.Change, value);
        //    }
        //    [MethodImpl(32)]
        //    remove
        //    {
        //        this.Change = (HistoryList.ChangeEventHandler)Delegate.Remove(this.Change, value);
        //    }
        //}
		public object NextState
		{
			get
			{
				this.AssertValid();
				if (!this.HasNextState)
				{
					throw new InvalidOperationException("HistoryList.NextState: There is no next state.  Check HasNextState before calling this.");
				}
				object result = this.m_oStateList[this.m_iCurrentObjectIndex + 1];
				this.m_iCurrentObjectIndex++;
				this.AssertValid();
				this.FireChangeEvent();
				return result;
			}
		}
		public object PreviousState
		{
			get
			{
				this.AssertValid();
				if (!this.HasPreviousState)
				{
					throw new InvalidOperationException("HistoryList.PreviousState: There is no previous state.  Check HasPreviousState before calling this.");
				}
				object result = this.m_oStateList[this.m_iCurrentObjectIndex - 1];
				this.m_iCurrentObjectIndex--;
				this.AssertValid();
				this.FireChangeEvent();
				return result;
			}
		}
		public bool HasNextState
		{
			get
			{
				this.AssertValid();
				return this.m_iCurrentObjectIndex < this.m_oStateList.Count - 1;
			}
		}
		public bool HasPreviousState
		{
			get
			{
				this.AssertValid();
				return this.m_iCurrentObjectIndex > 0;
			}
		}
		public HistoryList()
		{
			this.m_oStateList = new ArrayList();
			this.m_iCurrentObjectIndex = -1;
			this.AssertValid();
		}
		public object InsertState(object oState)
		{
			Debug.Assert(oState != null);
			this.AssertValid();
			this.m_oStateList.RemoveRange(this.m_iCurrentObjectIndex + 1, this.m_oStateList.Count - this.m_iCurrentObjectIndex - 1);
			this.m_oStateList.Add(oState);
			this.m_iCurrentObjectIndex++;
			this.AssertValid();
			Debug.Assert(this.m_iCurrentObjectIndex == this.m_oStateList.Count - 1);
			this.FireChangeEvent();
			return oState;
		}
		public void Reset()
		{
			this.m_oStateList.Clear();
			this.m_iCurrentObjectIndex = -1;
			this.FireChangeEvent();
			this.AssertValid();
		}
		protected void FireChangeEvent()
		{
			if (this.Change != null)
			{
				EventArgs oEventArgs = new EventArgs();
				this.Change(this, oEventArgs);
			}
		}
		public override string ToString()
		{
			this.AssertValid();
			return string.Concat(new object[]
			{
				"HistoryList object: Number of state objects: ", 
				this.m_oStateList.Count, 
				".  Current object:", 
				this.m_iCurrentObjectIndex, 
				"."
			});
		}
		[Conditional("DEBUG")]
		public void AssertValid()
		{
			Debug.Assert(this.m_oStateList != null);
			Debug.Assert(this.m_iCurrentObjectIndex >= -1);
			if (this.m_iCurrentObjectIndex >= 0)
			{
				Debug.Assert(this.m_iCurrentObjectIndex < this.m_oStateList.Count);
			}
		}
	}
}
