using System;
using System.Collections.Generic;

namespace SharedLibrary.Dto
{
    public class ErrorDto  // Ortak kullanılacak
    {
        public List<string> Errors { get; private set; }  // Dışarıdan set edilmesin
        public bool IsShow { get; private set; }

        // Parametresiz constructor, Errors listesini initialize ediyor
        public ErrorDto()
        {
            Errors = new List<string>();
        }

        // Tek hata mesajı alan constructor
        public ErrorDto(string error, bool isShow)
        {
            Errors = new List<string> { error };  // Listeyi initialize edip hatayı ekliyoruz
            IsShow = isShow;
        }

        // Birden fazla hata mesajı alan constructor
        public ErrorDto(List<string> errors, bool isShow)
        {
            Errors = errors ?? new List<string>();  // Null kontrolü yapılıyor
            IsShow = isShow;
        }
    }
}
