using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WestMarchSite.Infrastructure;

namespace WestMarchSite.Core
{
    public class SessionEntity
    {
        //public SessionStates SessionState { get; private set; }

        public string HostKey { get; }
        public string LeadKey { get; }
        public string PlayerKey { get; }

        public string Title { get; private set; }
        public string Description { get; private set; }

        public string LeadName { get; private set; }
        public SessionSchedule LeadSchedule { get; private set; }

        public string HostName { get; private set; }
        public SessionSchedule HostSchedule { get; private set; }

        public IEnumerable<Player> Players => _playerList;
        private List<Player> _playerList;

        public bool IsValid => throw new NotImplementedException();
        private List<string> ValidationErrors;

        public SessionEntity()
        {
            ValidationErrors = new List<string>();
            _playerList = new List<Player>();
        }

        //public void UpdateState(SessionStates newState)
        //{
        //    switch (this.SessionState)
        //    {
        //        case SessionStates.Created:
        //            if (newState != SessionStates.UnApproved)
        //                this.ValidationErrors.Add($"cannot move from created to {newState}");
        //            else if (string.IsNullOrWhiteSpace(this.LeadName))
        //                this.ValidationErrors.Add("lead must be set to move to seek approval");
        //            else
        //            {
        //                this.SessionState = newState;
        //            }
        //            return;
        //        case SessionStates.UnApproved:
        //            if (newState != SessionStates.Approved)
        //                this.ValidationErrors.Add($"cannot move from unapproved to {newState}");
        //            else if (string.IsNullOrWhiteSpace(this.HostName) || (!this.HostSchedule?.Options?.Any() ?? true))
        //                this.ValidationErrors.Add("host and their schedule must be set to be approved");
        //            else
        //            {
        //                this.SessionState = newState;
        //            }
        //            return;
        //        case SessionStates.Approved:
        //            if (newState != SessionStates.Open)
        //                this.ValidationErrors.Add($"cannot move from approved to {newState}");
        //            else if (this.lead)
        //            return;
        //        case SessionStates.Open:
        //            break;
        //        case SessionStates.Scheduled:
        //            break;
        //    }
        //}

        public void SetInfo(string title, string description)
        {
            //if (this.SessionState != SessionStates.Created)
            //    this.ValidationErrors.Add("info cannot be set unless creating a session");
            //else 
            if (!string.IsNullOrEmpty(this.Title))
                this.ValidationErrors.Add("title already populated");
            else if (!string.IsNullOrEmpty(this.Description))
                this.ValidationErrors.Add("description already populated");
            else
            {
                this.Title = title;
                this.Description = description;
            }
        }

        public void SetLead(string leadName)
        {
            if (!string.IsNullOrEmpty(this.LeadName))
                this.ValidationErrors.Add("lead is already set");
            else if (!string.IsNullOrWhiteSpace(LeadName))
                this.ValidationErrors.Add("lead's name must not be blank");
            else
            {
                this.LeadName = LeadName;
            }
        }

        public void SetHost(string hostName)
        {
            if (!string.IsNullOrEmpty(this.HostName))
                this.ValidationErrors.Add("host is already set");
            else
            {
                this.HostName = HostName;
            }
        }

        public void SetHostSchedule(SessionSchedule schedule)
        {
            //can't think of any validation errors? maybe state once it's set
            this.HostSchedule = schedule;
        }

        //TODO more validation errors below here
        public void SetLeadSchedule(SessionSchedule sessionSchedules)
        {
            this.LeadSchedule = sessionSchedules;
        }

        public void AddPlayer(string name, SessionSchedule schedule)
        {
            this._playerList.Add(new Player(name, schedule));
        }

        public void SetFinalSchedule(HostFinalizeDto finalizeDto)
        {
            this.
        }

        public void Finalize()
        {
            
        }
    }

    public enum SessionStates
    {
        Created,
        UnApproved,
        Approved,
        Open,
        Closed
    }
}
