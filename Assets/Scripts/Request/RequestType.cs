using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RequestType
{
    BorrowBook, // ���ϴ� å�� �ޱ� ���� Request ����
    ReturnBook, // Reading ������ ReturnBook �ؾ��ϴ� ����. ���� �Ϸ� �� Ready���� ���ư�
    FindBook, // å�忡�� å�� ã���ְ� �Ϸ��ϸ� �ٽ� Ready�� ���ư�.
    QuietDown, // ��ȣ�ۿ��ؼ� �ò����� �ʰ� �ؾ��ϴ� ����
    Reading, // BorrowBook �Ϸ� �� å Reading ����. �˾Ƽ� Ÿ�̸Ӱ� ������ Return���� ��
    Ready // ���� ������Ʈ ��� ����
}
