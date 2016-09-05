using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.TileList.Circular
{
    // Describe a circular profile
    public class CircularProfile
    {
        public double Radius { get; private set; }

        /// <summary>
        /// Describe a circular profile step
        /// </summary>
        public class ProfileStep
        {
            /// <summary>
            /// Trigonometric angle
            /// </summary>
            public double Angle { get; internal set; }

            /// <summary>
            /// Distance from center 
            /// </summary>
            public double Radius { get; internal set; }

            internal ProfileStep(ProfileStep s)
            {
                this.Angle = s.Angle;
                this.Radius = s.Radius;
            }

            public ProfileStep(double angle, double radius)
            {
                this.Angle = angle;
                this.Radius = radius;
            }

            public override string ToString()
            {
                return this.Angle + ";" + this.Radius;
            }
        }


        private List<ProfileStep> profile;

        public IEnumerable<ProfileStep> Profile
        {
            get
            {
                if (this.profile.Count > 0)
                {
                    return this.profile.OrderBy(p => p.Angle);
                }
                else
                {
                    return new ProfileStep[] { new ProfileStep(0d, this.Radius) };
                }
            }
        }

        public CircularProfile(double radius)
        {
            this.Radius = radius;
            this.ResetProfile();
        }

        public ProfileStep GetFirst()
        {
            ProfileStep last = new ProfileStep(this.Profile.Last());

            while (last.Angle > -Math.PI)
            {
                last.Angle = last.Angle - 2d * Math.PI;
            }

            return last;
        }

        public double GetMinRadius()
        {
            return this.Profile.Min<ProfileStep>(p => p.Radius);
        }

        public double GetMaxRadius()
        {
            return this.Profile.Max<ProfileStep>(p => p.Radius);
        }

        public double GetRadius()
        {
            return this.Profile.Sum<ProfileStep>(p => p.Radius) / this.Profile.Count();
        }

        public ProfileStep GetStep(double angle)
        {
            return this.Profile.LastOrDefault(p => p.Angle < angle) ?? this.GetFirst();
        }

        public void ResetProfile()
        {
            this.profile = new List<ProfileStep>();
            //       this.AddProfileStep(-Math.PI, this.Radius);
        }

        public void AddProfileStep(ProfileStep step)
        {
            this.profile.Add(step);
        }

        /// <summary>
        /// Add profile step
        /// </summary>
        /// <param name="angle">Angle from which radius change (in radian)</param>
        /// <param name="radius">Radius of selection from specified angle to next step (to end of circle if last step)</param>
        public void AddProfileStep(double angle, double radius)
        {
            this.profile.Add(new ProfileStep(angle, radius));

        }

        /// <summary>
        /// Add profile zone
        /// </summary>
        /// <param name="angle">Zone center angle</param>
        /// <param name="thickness">Radius delta relative to profile base radius for all zone</param>
        /// <param name="length">Zone length, centered on specified angle</param>
        public void AddProfileZone(double angle, double thickness, double length)
        {
            double delta = Math.Atan2(length / 2d, this.Radius);

            double angle_0 = angle - delta;
            double rayon_0 = this.Radius - thickness;

            this.AddProfileStep(angle_0, rayon_0);

            double angle_1 = angle + delta;
            double rayon_1 = this.Radius;

            this.AddProfileStep(angle_1, rayon_1);
        }

        /// <summary>
        /// Add tengential flat
        /// </summary>
        /// <param name="angle">Flat center angle (flat is orthogonal to radius at this angle)</param>
        /// <param name="thickness">Radius delta relative to profile base radius at specified angle</param>
        /// <param name="step">Calculation step (Profile is curved between each step)</param> 
        /// <param name="resolution">Calculation step (Profile is curved between each step)</param>
        public void AddProfileFlatByThickness(double angle, double thickness, double step = 1d, double resolution = 1d)
        {
            // Rayon central
            double r0 = this.Radius - thickness;

            // Arc de demi-flat
            double delta_flat = Math.Acos(r0 / this.Radius);

            // flat
            double length = 2d * this.Radius * Math.Sin(delta_flat);

            this.AddProfileFlat(angle, r0, length, step, resolution);
        }

        /// <summary>
        /// Add tengential flat
        /// </summary>
        /// <param name="angle">Flat center angle (flat is orthogonal to radius at this angle)</param>
        /// <param name="length">Flat lenght, centered on specified angle</param>
        /// <param name="step">Calculation step (Profile is curved between each step)</param>
        /// <param name="resolution">Calculation step (Profile is curved between each step)</param>
        public void AddProfileFlatByLength(double angle, double length, double step = 1d, double resolution = 1d)
        {
            // Arc de demi-flat
            double delta_flat = Math.Atan2(length / 2d, this.Radius);

            // Rayon central
            double r0 = Math.Cos(delta_flat) * this.Radius;

            this.AddProfileFlat(angle, r0, length, step, resolution);
        }

        /// <summary>
        /// Add tengential flat
        /// </summary>
        /// <param name="angle">Flat center angle (flat is orthogonal to radius at this angle)</param>
        /// <param name="radius">Profile base radius at specified angle</param>
        /// <param name="length">Flat lenght, centered on specified angle</param>
        /// <param name="step">Calculation step (Profile is curved between each step)</param>
        /// <param name="resolution">Calculation step (Profile is curved between each step)</param>
        public void AddProfileFlat(double angle, double radius, double length, double step = 1d, double resolution = 1d)
        {
            // Arc de demi-flat
            double delta_flat = Math.Atan2(length / 2d, this.Radius);

            // Rayon orthogonal au flat, corrigé selon la bordure
            double r0 = radius;

            double delta = Math.Atan2(step, this.Radius) * resolution;

            double angle_0;
            double rayon_0;

            // Nb de points de calcul
            int steps = (int)Math.Round(delta_flat / delta);

            // Debut du flat: les arcs partent du rayon considéré DANS le flat: OK
            for (int s = -steps; s < 0; s++)
            {
                angle_0 = angle + s * delta;
                rayon_0 = r0 / Math.Cos(angle_0 - angle);

                this.AddProfileStep(angle_0, rayon_0);
            }

            // Fin du flat: les arcs partent du rayon considéré HORS du flat: NOK
            // => il faut considérer le rayon de l'angle suivant pour que l'arc soit DANS le flat
            for (int s = 0; s < steps; s++)
            {
                angle_0 = angle + s * delta;
                rayon_0 = r0 / Math.Cos(angle_0 - angle + delta);

                this.AddProfileStep(angle_0, rayon_0);
            }

            // Dernier point
            angle_0 = angle + delta_flat;
            rayon_0 = this.Radius;

            this.AddProfileStep(angle_0, rayon_0);
        }
    }
}
