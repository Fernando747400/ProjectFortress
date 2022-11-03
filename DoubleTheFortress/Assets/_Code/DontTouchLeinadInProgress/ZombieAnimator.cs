using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ZombieAnimator : MonoBehaviour
{
    public enum AnimationsEnum { Idle, Walk, Death, Hit, Attack, Dance }   
    public AnimationsEnum CurrentAnimation;
 
    private Animator _zombieAnimator;
    private const string IdleAnim = "IdleZombie";
    private const string WalkAnim = "Zombie Walk";
    private const string DeathAnim = "DeathZombie";
    private const string HitAnim = "HitDamage";
    private const string AttackAnim = "ZombieAttack";
    private const string DanceAnim = "Happy zombie";

    private void Awake()
    {
        _zombieAnimator = GetComponent<Animator>();
    }

    public void PlayAnimation(AnimationsEnum animationEnum)
    {
        if (animationEnum == CurrentAnimation) return;
        
            CurrentAnimation = animationEnum;

        switch (animationEnum)
        {
            case AnimationsEnum.Idle:
                _zombieAnimator.Play(IdleAnim);
                break;
            case AnimationsEnum.Walk:
                _zombieAnimator.Play(WalkAnim);
                break;
            case AnimationsEnum.Death:
                _zombieAnimator.Play(DeathAnim);
                break;
            case AnimationsEnum.Hit:
                _zombieAnimator.Play(HitAnim);
                break;
            case AnimationsEnum.Attack:
                _zombieAnimator.Play(AttackAnim);
                break;
            case AnimationsEnum.Dance:
                _zombieAnimator.Play(DanceAnim);
                break;
        }
    }
}
