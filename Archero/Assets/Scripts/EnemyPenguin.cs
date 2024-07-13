using System.Collections;
using UnityEngine;

public class EnemyPenguin : MonoBehaviour
{
    RoomCondition RoomConditionGO;
    GameObject Player;
    LineRenderer lr;

    public LayerMask layerMask;
    public Transform BoltGenPosition; //발사체 생성위치
    public GameObject EnemyBolt;  //발사체
    public bool lookAtPlayer = true;

    void Start ( )
    {
        lr = GetComponent<LineRenderer> ( );
        Player = GameObject.FindGameObjectWithTag ( "Player" );
        RoomConditionGO = transform.parent.transform.parent.gameObject.GetComponent<RoomCondition> ( );

        lr.startColor = new Color ( 1, 0, 0, 0.5f );
        lr.endColor = new Color ( 1, 0, 0, 0.5f );
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;

        StartCoroutine ( WaitPlayer ( ) );
    }

    IEnumerator WaitPlayer ( )
    {
        yield return null;

        while ( !RoomConditionGO.playerInThisRoom )
        {
            yield return new WaitForSeconds ( 0.5f );
        }

        yield return new WaitForSeconds ( 0.5f );
        StartCoroutine ( SetTarget ( ) );

        yield return new WaitForSeconds ( 2f );
        DangerMarkerDeactive ( );
        Shoot ( );
    }

    IEnumerator SetTarget ( )
    {
        while ( true )
        {
            //if ( !lookAtPlayer ) break; // Line   안지워짐
            yield return null;
            if ( !lookAtPlayer ) break; // 이 위치 Line 지워짐

            Debug.Log ( " Set Player.transform.position " );
            transform.LookAt ( Player.transform.position );
            DangerMarkerShoot ( );
        }
    }

    public void DangerMarkerShoot ( )
    {
        Vector3 NewPosition = BoltGenPosition.position;
        Vector3 NewDir = transform.forward;
        lr.positionCount = 1;
        lr.SetPosition ( 0, transform.position );
        for ( int i = 1 ; i < 4 ; i++ )
        {
            Physics.Raycast ( NewPosition, NewDir, out RaycastHit hit, 30f, layerMask );

            Debug.Log ( " name : " + hit.transform.name + " position : " + hit.point );

            lr.positionCount++;
            Debug.Log ( " position : " + hit.point );
            lr.SetPosition ( i, hit.point );

            NewPosition = hit.point;
            NewDir = Vector3.Reflect ( NewDir, hit.normal );
        }
    }

    public void DangerMarkerDeactive ( )
    {
        lookAtPlayer = false;
        Debug.Log ( " lr.positionCount : " + lr.positionCount );
        for ( int i = 0 ; i < lr.positionCount ; i++ )
        {
            Debug.Log ( " Set Vector3.zero" );
            lr.SetPosition ( i, Vector3.zero );
        }
        lr.positionCount = 0;
    }

    public void Shoot ( )
    {
        Vector3 CurrentRotation = transform.eulerAngles + new Vector3 ( -90, 0, 0 );
        Instantiate ( EnemyBolt, BoltGenPosition.position, Quaternion.Euler ( CurrentRotation ) );
    }
}