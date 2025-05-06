using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RequestType
{
    BorrowBook, // 원하는 책을 받기 위한 Request 상태
    ReturnBook, // Reading 끝난후 ReturnBook 해야하는 상태. 상태 완료 후 Ready으로 돌아감
    FindBook, // 책장에서 책을 찾아주고 완료하면 다시 Ready로 돌아감.
    QuietDown, // 상호작용해서 시끄럽지 않게 해야하는 상태
    Reading, // BorrowBook 완료 후 책 Reading 상태. 알아서 타이머가 끝나면 Return으로 감
    Ready // 다음 리퀘스트 대기 상태
}
