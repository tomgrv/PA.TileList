using System;
using System.Collections.Generic;
using System.Linq;
using PA.TileList.Selection;
using PA.TileList.Quantified;

namespace PA.TileList.Circular
{
    // Describe a circular profile
    public class CircularProfile : ISelectionProfile
    {
        private ProfileStep _first;

        private ProfileStep _last;
        private double _maxRadius2;

        private double _minRadius2;

        private ProfileStep[] _ordered;

        private List<ProfileStep> _profile;

        public CircularProfile(double radius, string name = null)
        {
            Radius = radius;
            Name = name;
            ResetProfile();
        }

        public double Radius { get; }

        public IEnumerable<ProfileStep> Profile
        {
            get
            {
                return _ordered;
            }
        }

        public string Name { get; }

        public void OptimizeProfile()
        {
            if (_profile.Count == 0)
            {
                _ordered = new[] { new ProfileStep(0d, Radius) };
                _maxRadius2 = _minRadius2 = Math.Pow(Radius, 2);
            }

            if (_ordered == null)
            {
                _ordered = _profile.OrderBy(p => p.Angle).ToArray();

                var minRadius = _ordered.Min(p => p.Radius);
                var maxRadius = _ordered.Max(p => p.Radius);

                _maxRadius2 = Math.Pow(maxRadius, 2);
                _minRadius2 = Math.Pow(minRadius, 2);
            }
        }

        public SelectionPosition Position(double[] x, double[] y, bool IsQuickMode = false)
        {
            return Position(x[0], y[0], x[1], y[1],IsQuickMode);
        }

        public double[] GetValuesX(double x)
        {
            return new[] { x, x * x };
        }

        public double[] GetValuesY(double y)
        {
            return new[] { y, y * y };
        }

        public SelectionPosition Position(double x, double y,bool IsQuickMode = false)
        {
            return Position(x, y, x * x, y * y,IsQuickMode);
        }

        private SelectionPosition Position(double x, double y, double x2, double y2,
           bool IsQuickMode = false)
        {
            var angle = Math.Atan2(y, x);
            var r2 = x2 + y2;

            if (0 < _minRadius2 && r2 < _minRadius2)
                return SelectionPosition.Inside;

            if (0 < _maxRadius2 && r2 > _maxRadius2)
                return SelectionPosition.Outside;

            if (IsQuickMode) return SelectionPosition.Under;

            var last = GetStep(angle);
            var last2 = Math.Pow(last.Radius, 2);

            if (r2 < last2)
                return SelectionPosition.Inside;

            if (r2 > last2)
                return SelectionPosition.Outside;

            return SelectionPosition.Under;
        }

        public ProfileStep GetLast()
        {
            if (_last == null)
                _last = new ProfileStep(Math.PI, Profile.Last().Radius);

            return _last;
        }

        public ProfileStep GetFirst()
        {
            if (_first == null)
                _first = new ProfileStep(-Math.PI, GetLast().Radius);

            return _first;
        }

        public double GetMinRadius()
        {
            return Profile.Min(p => p.Radius);
        }

        public double GetMaxRadius()
        {
            return Profile.Max(p => p.Radius);
        }

        public double GetRadius()
        {
            return Profile.Sum(p => p.Radius) / Profile.Count();
        }

        public ProfileStep GetStep(double angle)
        {
            return Profile.LastOrDefault(p => p.Angle < angle) ?? GetLast();
        }

        /// <summary>
        ///     Resets the profile.
        /// </summary>
        public void ResetProfile()
        {
            _ordered = null;
            _last = null;
            _profile = new List<ProfileStep>();
        }

        /// <summary>
        ///     Adds the profile step.
        /// </summary>
        /// <param name="step">Step.</param>
        public void AddProfileStep(ProfileStep step)
        {
            _ordered = null;
            _last = null;
            _profile.Add(step);
        }

        /// <summary>
        ///     Add profile step
        /// </summary>
        /// <param name="angle">Angle from which radius change (in radian)</param>
        /// <param name="radius">Radius of selection from specified angle to next step (to end of circle if last step)</param>
        public void AddProfileStep(double angle, double radius)
        {
            AddProfileStep(new ProfileStep(angle, radius));
        }

        /// <summary>
        ///     Add profile zone
        /// </summary>
        /// <param name="angle">Zone center angle</param>
        /// <param name="thickness">Radius delta relative to profile base radius for all zone</param>
        /// <param name="length">Zone length, centered on specified angle</param>
        public void AddProfileZone(double angle, double thickness, double length)
        {
            var delta = Math.Atan2(length / 2d, Radius);

            var angle_0 = angle - delta;
            var rayon_0 = Radius - thickness;

            AddProfileStep(angle_0, rayon_0);

            var angle_1 = angle + delta;
            var rayon_1 = Radius;

            AddProfileStep(angle_1, rayon_1);
        }

        /// <summary>
        ///     Add tengential flat
        /// </summary>
        /// <param name="angle">Flat center angle (flat is orthogonal to radius at this angle)</param>
        /// <param name="thickness">Radius delta relative to profile base radius at specified angle</param>
        /// <param name="step">Calculation step (Profile is curved between each step)</param>
        /// <param name="resolution">Calculation step (Profile is curved between each step)</param>
        public void AddProfileFlatByThickness(double angle, double thickness, double step = 1d, double resolution = 1d)
        {
            // Rayon central
            var r0 = Radius - thickness;

            // Arc de demi-flat
            var delta_flat = Math.Acos(r0 / Radius);

            // flat
            var length = 2d * Radius * Math.Sin(delta_flat);

            AddProfileFlat(angle, r0, length, step, resolution);
        }

        /// <summary>
        ///     Add tengential flat
        /// </summary>
        /// <param name="angle">Flat center angle (flat is orthogonal to radius at this angle)</param>
        /// <param name="length">Flat lenght, centered on specified angle</param>
        /// <param name="step">Calculation step (Profile is curved between each step)</param>
        /// <param name="resolution">Calculation step (Profile is curved between each step)</param>
        public void AddProfileFlatByLength(double angle, double length, double step = 1d, double resolution = 1d)
        {
            // Arc de demi-flat
            var delta_flat = Math.Atan2(length / 2d, Radius);

            // Rayon central
            var r0 = Math.Cos(delta_flat) * Radius;

            AddProfileFlat(angle, r0, length, step, resolution);
        }

        /// <summary>
        ///     Add tengential flat
        /// </summary>
        /// <param name="angle">Flat center angle (flat is orthogonal to radius at this angle)</param>
        /// <param name="radius">Profile base radius at specified angle</param>
        /// <param name="length">Flat lenght, centered on specified angle</param>
        /// <param name="step">Calculation step (Profile is curved between each step)</param>
        /// <param name="resolution">Calculation step (Profile is curved between each step)</param>
        public void AddProfileFlat(double angle, double radius, double length, double step = 1d, double resolution = 1d)
        {
            // Arc de demi-flat
            var delta_flat = Math.Atan2(length / 2d, Radius);

            // Rayon orthogonal au flat, corrigé selon la bordure
            var r0 = radius;

            var delta = Math.Atan2(step, Radius) * resolution;

            double angle_0;
            double rayon_0;

            // Nb de points de calcul
            var steps = (int)Math.Round(delta_flat / delta);

            // Debut du flat: les arcs partent du rayon considéré DANS le flat: OK
            for (var s = -steps; s < 0; s++)
            {
                angle_0 = angle + s * delta;
                rayon_0 = r0 / Math.Cos(angle_0 - angle);

                AddProfileStep(angle_0, rayon_0);
            }

            // Fin du flat: les arcs partent du rayon considéré HORS du flat: NOK
            // => il faut considérer le rayon de l'angle suivant pour que l'arc soit DANS le flat
            for (var s = 0; s < steps; s++)
            {
                angle_0 = angle + s * delta;
                rayon_0 = r0 / Math.Cos(angle_0 - angle + delta);

                AddProfileStep(angle_0, rayon_0);
            }

            // Dernier point
            angle_0 = angle + delta_flat;
            rayon_0 = Radius;

            AddProfileStep(angle_0, rayon_0);
        }

        /// <summary>
        ///     Describe a circular profile step
        /// </summary>
        public class ProfileStep
        {
            internal ProfileStep(ProfileStep s)
            {
                Angle = s.Angle;
                Radius = s.Radius;
            }

            public ProfileStep(double angle, double radius)
            {
                SetAngle(angle);
                Radius = radius;
            }

            /// <summary>
            ///     Trigonometric angle
            /// </summary>
            public double Angle { get; private set; }

            /// <summary>
            ///     Distance from center
            /// </summary>
            public double Radius { get; }

            public override string ToString()
            {
                return Angle + ";" + Radius;
            }

            public void SetAngle(double angle)
            {
                Angle = angle;

                while (Angle < -Math.PI)
                    Angle += 2f * Math.PI;

                while (Angle > Math.PI)
                    Angle -= 2f * Math.PI;
            }
        }
    }
}