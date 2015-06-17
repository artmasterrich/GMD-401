using UnityEngine;
using System.Collections;

public class BattleCoroutine : MonoBehaviour {

	public void StartBattle( )
	{
		StartCoroutine(BattleRecursive(0));
	}
	
	public IEnumerator BattleRecursive( int depth )
	{
		// Start Coroutine"
		
		yield return new WaitForSeconds(2f);
		
		if( depth == 0 )
			yield return StartCoroutine( BattleRecursive(depth+1) );
		
		if( depth == 1 )
			yield return StartCoroutine( BattleRecursive(depth+1) );
		
		Debug.Log( "MyCoroutine is now finished at depth " + depth );
	}
}
