using VContainer.Unity;

namespace Features.AppStarter.Scripts.DTO
{
    public struct EntryPointArguments
    {
        public VContainerSettings VContainerSettings;

        public EntryPointArguments(VContainerSettings vContainerSettings)
        {
            VContainerSettings = vContainerSettings;
        }
    }
}