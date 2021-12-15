﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WestMarchSite.Infrastructure;

namespace WestMarchSite.Core
{
    public class SessionEntity
    {
        public SessionStates SessionState { get; private set; }

        public string HostKey { get; }
        public string LeadKey { get; }
        public string PlayerKey { get; }

        public string Title { get; private set; }
        public string Description { get; private set; }

        public string LeadName { get; private set; }

        public string HostName { get; private set; }
        public SessionSchedule HostSchedule { get; private set; }

        public SessionSchedule OpenSchedule { get; private set; }

        public IEnumerable<Player> Players => _playerList;
        private List<Player> _playerList;

        public SessionSchedule FinalizedSchedule { get; private set; }

        public bool IsValid => !_validationErrors.Any();

        private List<string> _validationErrors;

        public SessionEntity()
        {
            SessionState = SessionStates.Created;
            _validationErrors = new List<string>();
            _playerList = new List<Player>();
        }

        public void ProgressState()
        {
            switch (this.SessionState)
            {
                case SessionStates.Created:
                    //gotta have all the info filled in before submission
                    if (string.IsNullOrWhiteSpace(this.LeadName))
                        this._validationErrors.Add("lead must be set to move to seek approval");
                    else
                    {
                        this.SessionState = SessionStates.UnApproved;
                    }
                    return;
                case SessionStates.UnApproved:
                    //gotta have the host and host's schedule filled in for review
                    if (string.IsNullOrWhiteSpace(this.HostName) || (!this.HostSchedule?.Options?.Any() ?? true))
                        this._validationErrors.Add("host and their schedule must be set to be approved");
                    else
                    {
                        this.SessionState = SessionStates.Approved;
                    }
                    return;
                case SessionStates.Approved:
                    //lead must submit their schedule, then it should be good to go
                    if (!this.OpenSchedule?.Options?.Any() ?? true)
                        this._validationErrors.Add("lead must have submitted their schedule to move to open");
                    else
                    {
                        this.SessionState = SessionStates.Open;
                    }
                    return;
                case SessionStates.Open:
                    //a final schedule must be set in order for the session to be finalized
                    if (!this.FinalizedSchedule?.Options?.Any() ?? true)
                        this._validationErrors.Add("finalzied schedule must be provided before session can be closed");
                    else
                    {
                        this.SessionState = SessionStates.Finalized;
                    }
                    return;
                case SessionStates.Finalized:
                    this._validationErrors.Add("cannot progress beyond closed state");
                    return;
            }
        }

        public void SetInfo(string title, string description)
        {
            if (!string.IsNullOrEmpty(this.Title))
                this._validationErrors.Add("title already populated");
            else if (!string.IsNullOrEmpty(this.Description))
                this._validationErrors.Add("description already populated");
            else
            {
                this.Title = title;
                this.Description = description;
            }
        }

        public void SetLead(string leadName)
        {
            if (!string.IsNullOrEmpty(this.LeadName))
                this._validationErrors.Add("lead is already set");
            else if (!string.IsNullOrWhiteSpace(LeadName))
                this._validationErrors.Add("lead's name must not be blank");
            else
            {
                this.LeadName = LeadName;
            }
        }

        public void SetHost(string hostName)
        {
            if (!string.IsNullOrEmpty(this.HostName))
                this._validationErrors.Add("host is already set");
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
            this.OpenSchedule = sessionSchedules;
        }

        public void AddPlayer(string name, SessionSchedule schedule)
        {
            this._playerList.Add(new Player(name, schedule));
        }

        public void SetFinalSchedule(SessionSchedule schedule)
        {
            this.FinalizedSchedule = schedule;
        }
    }

    public enum SessionStates
    {
        Created,
        UnApproved,
        Approved,
        Open,
        Finalized
    }
}