using NetModular.Lib.Options.Abstraction;
using NetModular.Lib.Quartz.Abstractions;

namespace NetModular.Module.Quartz.Infrastructure
{
    public class ModuleOptionsChangedEvent : IModuleOptionsChangedEvent<QuartzOptions>
    {
        private readonly IQuartzServer _server;

        public ModuleOptionsChangedEvent(IQuartzServer server)
        {
            _server = server;
        }

        public void OnChanged(QuartzOptions options, QuartzOptions oldOptions)
        {
            if (!options.Enabled && oldOptions.Enabled)
            {
                _server.Stop();
            }
            else if (options.Enabled && !oldOptions.Enabled)
            {
                _server.Start(options.GetQuartzProps());
            }
            else if (options.Enabled && oldOptions.Enabled && HasChanged(options, oldOptions))
            {
                _server.Stop();
                _server.Start(options.GetQuartzProps());
            }
        }

        private bool HasChanged(QuartzOptions options, QuartzOptions oldOptions)
        {
            return options.InstanceName != oldOptions.InstanceName || options.Provider != oldOptions.Provider ||
                   options.SerializerType != oldOptions.SerializerType || options.TablePrefix != oldOptions.TablePrefix;
        }
    }
}
