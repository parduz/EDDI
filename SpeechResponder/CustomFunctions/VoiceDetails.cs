using System;
using Cottle.Functions;
using Cottle.Values;
using EddiSpeechResponder.Service;
using EddiSpeechService;
using JetBrains.Annotations;
using System.Linq;

namespace EddiSpeechResponder.CustomFunctions
{
    [UsedImplicitly]
    public class VoiceDetails : ICustomFunction
    {
        public string name => "VoiceDetails";
        public FunctionCategory Category => FunctionCategory.Details;
        public string description => Properties.CustomFunctions_Untranslated.VoiceDetails;
        public NativeFunction function => new NativeFunction((values) =>
        {
            if (values.Count == 0)
            {
                return new ReflectionValue(SpeechService.Instance?.allVoices);
            }
            if (values.Count == 1)
            {
                EddiSpeechService.VoiceDetails result = null;
                if (SpeechService.Instance?.allVoices != null && !string.IsNullOrEmpty(values[0].AsString))
                {
                    result = SpeechService.Instance?.allVoices.SingleOrDefault(v => string.Equals(v.name, values[0].AsString, StringComparison.InvariantCultureIgnoreCase));
                }
                return new ReflectionValue(result ?? new object());
            }
            return "The VoiceDetails function is used improperly. Please review the documentation for correct usage.";
        }, 0, 1);
    }
}
