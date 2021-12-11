// ObjectPool.cs
//
// Supports spawning and unspawning game objects that are instantiated from a
// common prefab. Can be used preallocate objects to avoid calls to
// GameObject.Instantiate during gameplay. Can also create objects on demand
// (which it does if no objects are available in the pool).
//
/// <summary>
/// A general pool object for reusable game objects.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// NOTE: This script contains several lines compiled only for the Unity editor.
//		 These are string/name manipulations that are unnecessary on device builds.
//		 To potentially improve performance these operations will only run in editor mode.

public class
ObjectPool
{
    //
    // Class Members
    //

    private GameObject objectPrefab;		// the prefab every pooled object will instantiate
    private MonoBehaviour coroutineEnabler; // (optional) required to run coroutines

    private GameObject poolObjectParent;	// empty gameObject transform to store pooled gameObjects under
    private string poolObjectParentLabel;	// cached label for gameObject name

    private uint poolItemCount;				// internal iterator for number of pooled objects, used for gameObject name

    private Stack poolAvailable;			// all inactive/disabled objects in the pool
    private List<GameObject> poolActive;	// all active/enabled objects in the pool
    // NOTE: Must support index based retrieval for Destroy()

    //
    // Class Constructors
    //

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectPool"/> class.
    /// </summary>
    /// <param name='prefab'>
    /// Reference to the prefab asset to replicate.
    /// </param>
    /// <param name='capacity'>
    /// Initial maximum number of objects to expect in the scene.
    /// </param>
    /// <param name="coroutineEnabler">
    /// (Optional) MonoBehaviour instance to run coroutines from. Must be assigned to enable limited object lifetime.
    /// </param>
    public
    ObjectPool(GameObject prefab, int capacity, MonoBehaviour thisMonobehaviour = null)
    {
        objectPrefab = prefab;

        poolAvailable = new Stack(capacity);
        poolActive = new List<GameObject>(capacity);

        poolObjectParentLabel = objectPrefab.name + " Pool";
        poolObjectParent = new GameObject(poolObjectParentLabel + "(0/" + capacity + ")");
		GameObject.DontDestroyOnLoad(poolObjectParent);
		
        poolItemCount = 0;

        coroutineEnabler = thisMonobehaviour;

        PrePopulate(capacity);
    }

    //
    // Public Methods
    //

    /// <summary>
    /// Create a prefab at specified position.
    /// </summary>
    /// <param name='position'>
    /// World position.
    /// </param>
    /// <param name="lifeTime">
    /// (Optional) Time in seconds to persist before forcing deactivation.
    /// </param>
    public GameObject
    Create(Vector3 position,Vector3 scale,Transform parent, float lifeTime = float.PositiveInfinity)
    {
        return Create(position, scale,Quaternion.identity,parent, lifeTime);
    }


    /// <summary>
    /// Create a prefab at the specified position and rotation.
    /// </summary>
    /// <param name='position'>
    /// World Position.
    /// </param>
    /// <param name='rotation'>
    /// Rotation.
    /// </param>
    /// <param name="lifeTime">
    /// (Optional) Time in seconds to persist before forcing deactivation.
    /// </param>
    public GameObject
    Create(Vector3 position, Vector3 scale, Quaternion rotation,Transform parent, float lifeTime = float.PositiveInfinity)
    {
        //SystemConfig.MyLog("Creating object @ " + position + " with lifetime " + lifeTime);  

        GameObject _objectReference;

        // need to instantiate new object/raise capacity
        // NOTE: use of GameObject.Instantiate may cause undesirable performance on mobile devices!
        if (poolAvailable.Count == 0)
        {
            _objectReference = (GameObject)GameObject.Instantiate(objectPrefab, position, rotation);
            _objectReference.transform.parent = parent;
			_objectReference.transform.localPosition = position;
			_objectReference.transform.localRotation = rotation;
			_objectReference.transform.localScale = scale;
			
#if UNITY_EDITOR
			_objectReference.name = objectPrefab.name + poolItemCount++;
#endif

            poolActive.Add(_objectReference);
        }
        else
        {
            _objectReference = (GameObject)poolAvailable.Pop();

            Transform returnObjectTransform = _objectReference.transform;
            
			returnObjectTransform.parent = parent;
            returnObjectTransform.localPosition = position;
            returnObjectTransform.localRotation = rotation;
			_objectReference.transform.localScale = scale;

            _objectReference.SetActive(true);

            poolActive.Add(_objectReference);
        }

#if UNITY_EDITOR			
		poolObjectParent.name = poolObjectParentLabel + "(" + poolActive.Count + "/" + poolItemCount + ")";
#endif

        if (lifeTime != float.PositiveInfinity)
        {
            if (coroutineEnabler == null)
            {
                Debug.LogWarning("ObjectPool for '" + objectPrefab.name + "' requires a MonoBehaviour assigned to set limited lifetimes.");
            }
            else
                coroutineEnabler.StartCoroutine(SetLifeTime(_objectReference, lifeTime));
        }

        return _objectReference;
    }

    /// <summary>
    /// Remove a pooled object from the scene so that it can be recycled.
    /// </summary>
    /// <param name='objectReference'>
    /// Object reference.
    /// </param>
    public void
    Destroy(GameObject objectReference)
    {
        if (poolActive.Count == 0)
        {
            Debug.LogWarning("Trying to destroy " + objectReference.name + " but there are no active objects in pool!");
            return;
        }

		int _objectIndex = poolActive.IndexOf(objectReference);

		if (_objectIndex == -1)
		{
			Debug.LogError(objectReference.name + " instance not found in " + objectPrefab.name + " pool.");

			return;
		}
		else
		{
			objectReference = (GameObject)poolActive[_objectIndex];
			objectReference.SetActive(false);
			objectReference.transform.parent =  poolObjectParent.transform;
					
			poolAvailable.Push(objectReference);
			poolActive.RemoveAt(_objectIndex);

			#if UNITY_EDITOR			
			poolObjectParent.name = poolObjectParentLabel + "(" + poolActive.Count + "/" + poolItemCount + ")";
			#endif

		}
        
    }

	
	public void DestroyAllFromMem(){
        for (int i = 0; poolActive.Count != i; i++)
        {
            GameObject.Destroy( poolActive[i]);
        }
		poolActive.Clear();
		
		while(poolAvailable.Count != 0){
			GameObject.Destroy( (GameObject)poolAvailable.Pop());
		}
		poolAvailable.Clear();
		poolItemCount = 0;
#if UNITY_EDITOR
		poolObjectParent.name = poolObjectParentLabel + "(" + poolActive.Count + "/" + poolItemCount + ")";
#endif		
	}
	

    //
    // Private Methods
    //

    /// <summary>
    /// Should be called immediately after the ObjectPool constructor to allocate gameObjects in world.
    /// </summary>
    /// <param name='count'>
    /// Number of prefabs to instantiate.
    /// </param>
    public void
    PrePopulate(int count)
    {
        GameObject _objectReference;

        for (ushort i = 0; i < count; i++, poolItemCount++)
        {
            _objectReference = (GameObject)GameObject.Instantiate(objectPrefab, Vector3.zero, Quaternion.identity);
            _objectReference.SetActive(false);
            _objectReference.transform.parent = poolObjectParent.transform;
            //_objectReference.transform.localPosition = new Vector3(0, 5000, 0);
            //_objectReference.transform.localScale = Vector3.one;

#if UNITY_EDITOR
			_objectReference.name   = objectPrefab.name + poolItemCount;
#endif

            poolAvailable.Push(_objectReference);
        }
    }

    /// <summary>
    /// Sets a pooled object to be deactivated within a certain time frame using coroutines.
    /// </summary>
    /// <param name="objectReference">Object reference.</param>
    /// <param name="lifeTime">Time in seconds to persist before forcing deactivation.</param>
    /// <returns>Returns IEnumerator, but should not be stepped through.</returns>
    private IEnumerator
    SetLifeTime(GameObject objectReference, float lifeTime)
    {
        if (!objectReference.activeInHierarchy)
        {
            Debug.LogWarning("Object '" + objectReference + "' is trying to set lifetime, but is already inactive.");
            yield break;
        }

        if (lifeTime <= 0f)
        {
            Debug.LogWarning("Trying to set object " + objectReference + " lifetime to less than zero seconds. (Found " + lifeTime + ")");
            lifeTime = 0f;
        }

        yield return new WaitForSeconds(lifeTime);

        if (objectReference.activeSelf)
        {
            Destroy(objectReference);
        }
    }

    //
    // Method Overrides
    //

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="ObjectPool"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents the current <see cref="ObjectPool"/>.
    /// </returns>
    public override string
    ToString()
    {
        return "[ " + objectPrefab.name + " ObjectPool] " + "Size: " + (poolItemCount) + " (Active: " + poolActive.Count + ", Available: " + poolAvailable.Count + ")";
    }
}