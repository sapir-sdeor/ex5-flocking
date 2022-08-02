using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Avrahamy;
using Avrahamy.Math;
using BitStrap;

namespace Flocking {
    public class FollowerPeepController : MonoBehaviour {
        [SerializeField] PeepController peep;
        [SerializeField] float senseRadius = 2f;
        [SerializeField] PassiveTimer navigationTimer;
        [SerializeField] LayerMask navigationMask;
        [TagSelector]
        [SerializeField] string peepTag;
        [SerializeField] bool repelFromSameGroup;
        public static Vector3 redWeights = new Vector3(0f, 0.2f, 0.8f); 
        public static Vector3 blueWeights = new Vector3(0f, 0.2f, 0.8f);
        
        private Vector3 alignmentSum;
        private Vector3 cohesionSum;
        private int peepCounter;
        private int allPeepsCounter;
        private Vector3 alignmentVector;
        private Vector3 cohesionVector;
        private Vector3 separationVector;
        private Transform leaderTransform;
        

        private static readonly Collider[] COLLIDER_RESULTS = new Collider[10];

        protected void Reset() {
            if (peep == null) {
                peep = GetComponent<PeepController>();
            }
        }

        protected void OnEnable() {
            navigationTimer.Start();
            peep.DesiredVelocity = Random.insideUnitCircle.normalized;
        }

        protected void Update()
        {
            //initialized every update
            cohesionSum = Vector3.zero;
            alignmentSum = Vector3.zero;
            peepCounter = 0;
            allPeepsCounter = 0;
            
            if (navigationTimer.IsActive) return;
            navigationTimer.Start();
            var position = peep.Position;

            // Get all the peeps in the game, for the cohesion flocking
            var cohesionHits = Physics.OverlapSphereNonAlloc(
                position,
                Mathf.Infinity,
                COLLIDER_RESULTS,
                navigationMask.value);

            if (cohesionHits <= 1) return;
            var avgDirection = Vector3.zero;

            for (int j = 0; j < cohesionHits; j++)
            {
                var coHit = COLLIDER_RESULTS[j];
                // Ignore self.
                if (coHit.attachedRigidbody != null && coHit.attachedRigidbody.gameObject == peep.gameObject) continue;
                
               // Always repel from walls.
                if (coHit.CompareTag(peepTag))
                {
                    var otherPeed = coHit.attachedRigidbody.GetComponent<PeepController>();
                    // Ignore peeps that are not from this group.
                    if (otherPeed.Group != peep.Group) continue; 
                    
                    // Get the transform of the leader, so that when calculating cohesion he will get more weight
                    if (coHit.gameObject.layer == LayerMask.NameToLayer("Leader"))
                    {
                        leaderTransform = coHit.GetComponent<Transform>();
                    }
                    
                    cohesionSum += coHit.transform.position;
                    allPeepsCounter++;
                }
            }

           

            // Check for colliders in the sense radius.
            var hits = Physics.OverlapSphereNonAlloc(
                position,
                senseRadius,
                COLLIDER_RESULTS,
                navigationMask.value);

            // There will always be at least one hit on our own collider.
            if (hits <= 1) return;

            for (int i = 0; i < hits; i++) {
                var hit = COLLIDER_RESULTS[i];
                // Ignore self.
                if (hit.attachedRigidbody != null && hit.attachedRigidbody.gameObject == peep.gameObject) continue;

                // Always repel from walls.
                var repel = true;
                if (hit.CompareTag(peepTag)) {
                    // Sensed another peep.
                    var otherPeed = hit.attachedRigidbody.GetComponent<PeepController>();
                    // Ignore peeps that are not from this group.
                    if (otherPeed.Group != peep.Group) continue;
                    alignmentSum += hit.attachedRigidbody.velocity.normalized;
                    peepCounter++;
                    repel = repelFromSameGroup;
                }



                var closestPoint = hit.ClosestPoint(position);
                closestPoint.y = 0f;
                DebugDraw.DrawLine(
                    position + Vector3.up,
                    closestPoint + Vector3.up,
                    Color.cyan,
                    navigationTimer.Duration / 2);
                var direction = closestPoint - position;

                var magnitude = direction.magnitude;
                var distancePercent = repel
                    ? Mathf.InverseLerp(peep.SelfRadius, senseRadius, magnitude)
                    // Inverse attraction factor so peeps won't be magnetized to
                    // each other without being able to separate.
                    : Mathf.InverseLerp(senseRadius, peep.SelfRadius, magnitude);

                // Make sure the distance % is not 0 to avoid division by 0.
                distancePercent = Mathf.Max(distancePercent, 0.01f);

                // Force is stronger when distance percent is closer to 0 (1/x-1).
                var forceWeight = 1f / distancePercent - 1f;

                // Angle between forward to other collider.
                var angle = transform.forward.GetAngleBetweenXZ(direction);
                var absAngle = Mathf.Abs(angle);
                if (absAngle > 90f) {
                    // Decrease weight of colliders that are behind the peep.
                    // The closer to the back, the lower the weight.
                    var t = Mathf.InverseLerp(180f, 90f, absAngle);
                    forceWeight *= Mathf.Lerp(0.1f, 1f, t);
                }

                direction = direction.normalized * forceWeight;
                if (repel) {
                    avgDirection -= direction;
                    DebugDraw.DrawArrowXZ(
                        position + Vector3.up,
                        -direction * 3f,
                        1f,
                        30f,
                        Color.magenta,
                        navigationTimer.Duration / 2);
                } else {
                    avgDirection += direction;
                    DebugDraw.DrawArrowXZ(
                        position + Vector3.up,
                        direction * 3f,
                        1f,
                        30f,
                        Color.green,
                        navigationTimer.Duration / 2);
                }
            }
            if (avgDirection.sqrMagnitude < 0.1f) return;
            if (avgDirection.sqrMagnitude < 0.1f) return;
           
            separationVector = avgDirection;
            alignmentVector = alignmentSum / peepCounter;
            
            cohesionVector = cohesionSum / (allPeepsCounter + 1);
            // Give the leader more weight, and remove self from cohesion calculation
            if (leaderTransform != null)
                cohesionVector = (leaderTransform.position * 0.9f + cohesionVector * 0.1f) - peep.transform.position;
            Vector3 avg = Vector3.zero;
            if (peep.Group == 0)
               avg = separationVector * redWeights.x + alignmentVector * redWeights.y + cohesionVector * redWeights.z;
            else if (peep.Group == 1)
               avg = separationVector * blueWeights.x + alignmentVector * blueWeights.y + cohesionVector * blueWeights.z;
            peep.DesiredVelocity = (avg).normalized.ToVector2XZ();
        }
        
        

        protected void OnDrawGizmos() {
            var angle = transform.forward.GetAngleXZ();
            DebugDraw.GizmosDrawSector(
                transform.position,
                senseRadius,
                180f + angle,
                -180f + angle,
                Color.green);
        }
    }
}
