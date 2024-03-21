using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    public Animator _animator;
    private AnimationClip[] _animationClips;
    public AnimationClip[] AnimationClips => _animationClips;

    private Dictionary<string, int> _nameToHashPair = new Dictionary<string, int>();
    private Dictionary<string, int> _animDict = new Dictionary<string, int>();
    private void InitAnimPair()
    {
        _nameToHashPair.Clear();
        _animationClips = _animator.runtimeAnimatorController.animationClips;
        foreach (var clip in _animationClips)
        {
            int hash = Animator.StringToHash(clip.name);
            _nameToHashPair.Add(clip.name, hash);
        }
    }

    public void Init()
    {
        _animator = Util.GetOrAddComponent<Animator>(transform.GetChild(0).gameObject);
        _animator.runtimeAnimatorController = Managers.Resource.LoadPrefabAnimator();
        InitAnimPair();
    }

    public void PlayAnimation(string name)
    {
        if (_animDict.ContainsKey(name))
        {
            _animator.Play(_animDict[name], 0);
        }
        else
            foreach (var animationName in _nameToHashPair)
            {
                if (animationName.Key.ToLower().Contains(name.ToLower()))
                {
                    _animator.Play(animationName.Value, 0);
                    _animDict.Add(name, animationName.Value);
                    break;
                }
            }
    }
}
