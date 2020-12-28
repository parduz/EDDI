using Cottle.Functions;
using Cottle.Values;
using EddiSpeechResponder.Service;
using EddiSpeechService;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;

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
                List<VoiceDetail> voices = new List<VoiceDetail>();
                if (SpeechService.Instance?.allVoices != null)
                {
                    foreach (VoiceInfo vc in SpeechService.Instance.allVoices.Select(v => v.voiceInfo))
                    {
                        if (!vc.Name.Contains("Microsoft Server Speech Text to Speech Voice"))
                        {
                            voices.Add(new VoiceDetail(
                                vc.Name,
                                vc.Culture.Parent.EnglishName,
                                vc.Culture.Parent.NativeName,
                                vc.Culture.Name,
                                vc.Gender.ToString()
                                ));
                        }
                    }
                }
                return new ReflectionValue(voices);
            }
            if (values.Count == 1)
            {
                VoiceDetail result = null;
                if (SpeechService.Instance?.allVoices != null && !string.IsNullOrEmpty(values[0].AsString))
                {
                    foreach (VoiceInfo vc in SpeechService.Instance.allVoices.Select(v => v.voiceInfo))
                    {
                        if (vc.Name.ToLowerInvariant().Contains(values[0].AsString.ToLowerInvariant())
                        && !vc.Name.Contains("Microsoft Server Speech Text to Speech Voice"))
                        {
                            result = new VoiceDetail(
                                vc.Name,
                                vc.Culture.Parent.EnglishName,
                                vc.Culture.Parent.NativeName,
                                vc.Culture.Name,
                                vc.Gender.ToString()
                                );
                            break;
                        }
                    }
                }
                return new ReflectionValue(result ?? new object());
            }
            return "The VoiceDetails function is used improperly. Please review the documentation for correct usage.";
        }, 0, 1);
    }

    [PublicAPI]
    class VoiceDetail
    {
        public string name { get; }
        public string cultureinvariantname { get; }
        public string culturename { get; }
        public string culturecode { get; }
        public string gender { get; }

        public VoiceDetail(string name, string cultureinvariantname, string culturename, string culturecode, string gender)
        {
            this.name = name;
            this.cultureinvariantname = cultureinvariantname;
            this.culturename = culturename;
            this.culturecode = culturecode;
            this.gender = gender;
        }
    }
}
