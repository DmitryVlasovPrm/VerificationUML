using System;
using System.Collections.Generic;
using System.Linq;

namespace Verification.uc_ver
{
    internal class Checker
    {
        private readonly Dictionary<string, Element> elements;
        private readonly List<Mistake> mistakes;
        public Checker(Dictionary<string, Element> elements, List<Mistake> mistakes)
        {
            this.elements = elements;
            this.mistakes = mistakes;
        }
        public void Check()
        {
            CheckActors();
            CheckComments();
            СheckPackages();
            CheckPrecedents();
        }

        #region Checks
        private void CheckActors()
        {
            var actors = elements.Where(element => element.Value.Type == ElementTypes.Actor);
            foreach (var actorName in actors.GroupBy(a => a.Value.Name))
            {
                if (actorName.Count() > 1)
                {
                    var errorElements = elements
                        .Where(element => element.Value.Type == ElementTypes.Actor && element.Value.Name == actorName.Key);

                    foreach (var element in errorElements)
                    {
                        mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Имя актора повторяется: {actorName.Key}",
                            element.Value));
                    }
                }
                if (string.IsNullOrEmpty(actorName.Key.Trim()) || !char.IsUpper(actorName.Key[0]))
                {
                    var errorElements = elements
                       .Where(element => element.Value.Type == ElementTypes.Actor && element.Value.Name == actorName.Key);

                    foreach (var element in errorElements)
                    {
                        mistakes.Add(UCMistakeFactory.Create(
                           MistakesTypes.ERROR,
                           $"Имя актора должно быть представлено в виде существительного с заглавной буквы: {actorName.Key}",
                           element.Value));
                    }
                }
            }

            foreach (var actor in actors)
            {
                if (!HaveConnection(actor.Key, ElementTypes.Association))
                {
                    mistakes.Add(UCMistakeFactory.Create(
                           MistakesTypes.ERROR,
                           $"Актор не имеет ни одной связи типа ассоцияция с прецедентами: {actor.Value.Name}",
                           actor.Value));
                }
            }
        }

        private void CheckComments()
        {
            var comments = elements.Where(element => element.Value.Type == ElementTypes.Comment);
            foreach (var comment in comments)
                if (string.IsNullOrEmpty(comment.Value.Name.Trim()))
                {
                    mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                             $"Отсутствует текст в условии расширения",
                            comment.Value));
                }
        }

        private void СheckPackages()
        {
            var packages = elements.Where(element => element.Value.Type == ElementTypes.Package);

            if (packages.Count() == 0)
            {
                mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Отсутствует граница системы"));
            }


            foreach (var package in packages)
                if (string.IsNullOrEmpty(package.Value.Name.Trim()))
                {
                    mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Отсутствует назние системы",
                            package.Value));
                }
        }

        private void CheckPrecedents()
        {
            var extensionPoints = elements.Where(element => element.Value.Type == ElementTypes.ExtensionPoint);
            foreach (var point in extensionPoints)
                if (string.IsNullOrEmpty(point.Value.Name.Trim()))
                {
                    mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Отсутствует текст в точке расширения прецедента",
                            point.Value));
                }

            var precedents = elements.Where(element => element.Value.Type == ElementTypes.Precedent);
            foreach (var precedentName in precedents.GroupBy(p => p.Value.Name))
            {
                if (precedentName.Count() > 1)
                {
                    var errorElements = elements
                        .Where(element => element.Value.Type == ElementTypes.Precedent && element.Value.Name == precedentName.Key);

                    foreach (var element in errorElements)
                    {
                        mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Имя прецедента повторяется: {precedentName.Key}",
                            element.Value));
                    }
                }
                var words = precedentName.Key.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (string.IsNullOrEmpty(precedentName.Key.Trim()) || !char.IsUpper(precedentName.Key[0]) || !IsVerb(words[0]))
                {
                    var errorElements = elements
                        .Where(element => element.Value.Type == ElementTypes.Precedent && element.Value.Name == precedentName.Key);

                    foreach (var element in errorElements)
                    {
                        mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Имя прецедента должно быть представлено в виде действия, начинаясь с заглавной буквы: {precedentName.Key}",
                            element.Value));
                    }
                }
            }

            foreach (var precedent in precedents)
            {
                bool haveAssociation = HaveConnection(precedent.Value.Id, ElementTypes.Association),
                    haveGeneralization = HaveConnection(precedent.Value.Id, ElementTypes.Generalization),
                    haveExtendsion = HaveConnection(precedent.Value.Id, ElementTypes.Extend),
                    haveIncluding = HaveConnection(precedent.Value.Id, ElementTypes.Include);

                if (!haveAssociation && !haveGeneralization && !haveExtendsion && !haveIncluding)
                {
                    mistakes.Add(UCMistakeFactory.Create(
                        MistakesTypes.ERROR,
                        $"Прецедент должен иметь связь с актором в виде ассоциации," +
                        $" либо иметь отношения расширения," +
                        $" дополнения или включения с другими прецедентами: {precedent.Value.Name}",
                        precedent.Value));
                }

                if (haveExtendsion)
                {
                    bool havePoint = elements.Where(element =>
                    {
                        if (element.Value.Type != ElementTypes.ExtensionPoint) return false;
                        if (((Arrow)element.Value).To.Equals(precedent.Key))
                            return true;
                        return false;
                    }).Count() > 0;
                    bool extended = elements.Where(element =>
                    {
                        if (element.Value.Type != ElementTypes.Extend) return false;
                        if (((Arrow)element.Value).To.Equals(precedent.Key))
                            return true;
                        return false;
                    }).Count() > 0;

                    if (extended && !havePoint)
                    {
                        mistakes.Add(UCMistakeFactory.Create(
                            MistakesTypes.ERROR,
                            $"Отсутствие точки расширения у прецедента с связью расширения: {precedent.Value.Name}",
                            precedent.Value));
                    }
                }

                if (haveIncluding)
                {
                    int includesCount = elements.Where(element =>
                    {
                        if (element.Value.Type != ElementTypes.Include) return false;
                        if (((Arrow)element.Value).From.Equals(precedent.Key))
                            return true;
                        return false;
                    }).Count();

                    if (includesCount > 0 && includesCount < 2)
                    {
                        mistakes.Add(UCMistakeFactory.Create(
                           MistakesTypes.WARNING,
                           $"Прецедент включает всего один прецедент: {precedent.Value.Name}",
                           precedent.Value));
                    }
                }
            }
        }
        #endregion

        #region Support Functions
        private bool HaveConnection(string id, string type)
        {
            var assosiations = elements.Where(element => element.Value.Type == type);
            return assosiations.Where(a =>
            {
                if (((Arrow)a.Value).To.Equals(id) ||
                ((Arrow)a.Value).From.Equals(id))
                    return true;
                return false;
            }).Count() > 0;
        }

        private bool IsVerb(string name)
        {
            var isVerv = false;
            var ends = new List<string>() { "ть", "ся", "сь", "ти" };
            ends.ForEach(end => isVerv = isVerv || name.EndsWith(end));
            return isVerv;
        }
        #endregion
    }
}
