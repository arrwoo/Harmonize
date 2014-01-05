﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace Bebbs.Harmonize.With.Alljoyn
{
    internal class Harmonizer : IInitializeAtStartup, ICleanupAtShutdown
    {
        private readonly IGlobalEventAggregator _eventAggregator;
        private Bus.ICoordinator _busCoordinator;

        private IDisposable _subscription;

        public Harmonizer(IGlobalEventAggregator eventAggregator, Bus.ICoordinator busCoordinator)
        {
            _eventAggregator = eventAggregator;
            _busCoordinator = busCoordinator;
        }

        private void RegisterDevice(Message.RegisterDevice message)
        {
            _busCoordinator.Add(message.Device);
        }

        private void Started()
        {
            // Do nothing
        }

        private void Stopped()
        {
            // Do nothing
        }

        private void DeregisterDevice(Message.DeregisterDevice message)
        {
            _busCoordinator.Remove(message.Device);
        }

        public void Initialize()
        {
            _busCoordinator.Start();

            _subscription = new CompositeDisposable(
                _eventAggregator.GetEvent<With.Message.Started>().Subscribe(message => Started()),
                _eventAggregator.GetEvent<With.Message.Stopped>().Subscribe(message => Stopped()),
                _eventAggregator.GetEvent<With.Message.RegisterDevice>().Subscribe(RegisterDevice),
                _eventAggregator.GetEvent<With.Message.DeregisterDevice>().Subscribe(DeregisterDevice)
            );
        }

        public void Cleanup()
        {
            if (_subscription != null)
            {
                _subscription.Dispose();
                _subscription = null;
            }

            _busCoordinator.Stop();
        }
    }
}
