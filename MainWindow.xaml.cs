using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using StudentJournal.Models;

namespace StudentJournal
{

    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Student> _students = new();
        private readonly ObservableCollection<Group>   _groups   = new();
        private readonly ObservableCollection<Teacher> _teachers = new();
        private readonly ObservableCollection<Subject> _subjects = new();

        private int _nextStudentId = 1;
        private int _nextGroupId   = 1;
        private int _nextTeacherId = 1;
        private int _nextSubjectId = 1;

        public MainWindow()
        {
            InitializeComponent();
            RefreshAll();
        }

        private void RefreshAll()
        {
            GridStudents.ItemsSource  = null; GridStudents.ItemsSource  = _students;
            GridGroups.ItemsSource    = null; GridGroups.ItemsSource    = _groups;
            GridTeachers.ItemsSource  = null; GridTeachers.ItemsSource  = _teachers;
            GridSubjects.ItemsSource  = null; GridSubjects.ItemsSource  = _subjects;

            CmbSGroup.ItemsSource         = _groups;
            CmbGStudent.ItemsSource       = _students;
            CmbGSubject.ItemsSource       = _subjects;
            CmbTSubject.ItemsSource       = _subjects;
            CmbSearchGroup.ItemsSource    = _groups;
            CmbSearchSubject.ItemsSource  = _subjects;
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            if (CmbSGroup.SelectedItem is not Group group)
            {
                MessageBox.Show("Оберіть групу.", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var student = new Student
                {
                    Id        = _nextStudentId++,
                    FirstName = TxtSFirstName.Text,
                    LastName  = TxtSLastName.Text,
                    Group     = group
                };
                _students.Add(student);
                group.Students.Add(student);
                RefreshAll();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            if (GridStudents.SelectedItem is not Student student) return;

            student.Group?.Students.Remove(student);
            _students.Remove(student);
            ClearStudentPanel();
            RefreshAll();
        }

        private void UpdateStudent_Click(object sender, RoutedEventArgs e)
        {
            if (GridStudents.SelectedItem is not Student student) return;

            try
            {
                if (!string.IsNullOrWhiteSpace(TxtSFirstName.Text))
                    student.FirstName = TxtSFirstName.Text;

                if (!string.IsNullOrWhiteSpace(TxtSLastName.Text))
                    student.LastName = TxtSLastName.Text;

                if (CmbSGroup.SelectedItem is Group newGroup && newGroup != student.Group)
                {
                    student.Group?.Students.Remove(student);
                    student.Group = newGroup;
                    newGroup.Students.Add(student);
                }

                RefreshAll();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GridStudents_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (GridStudents.SelectedItem is not Student student)
            {
                ClearStudentPanel();
                return;
            }

            TxtSFirstName.Text = student.FirstName;
            TxtSLastName.Text  = student.LastName;

            GridStudentGrades.ItemsSource = student.Grades;

            TxtStudentName.Text   = student.FullName;
            TxtStudentGroup.Text  = $"Група: {student.Group?.Name ?? "—"}";
            TxtStudentAvg.Text    = $"Середній бал: {student.AverageGrade}";
            TxtStudentStatus.Text = student.IsPassing ? "✓ Успішний" : "✗ Неуспішний";
            TxtStudentCount.Text  = $"Предметів: {student.Grades.Count}";
        }

        private void ClearStudentPanel()
        {
            GridStudentGrades.ItemsSource = null;
            TxtStudentName.Text   = string.Empty;
            TxtStudentGroup.Text  = string.Empty;
            TxtStudentAvg.Text    = string.Empty;
            TxtStudentStatus.Text = string.Empty;
            TxtStudentCount.Text  = string.Empty;
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var group = new Group { Id = _nextGroupId++, Name = TxtGroupName.Text };
                _groups.Add(group);
                TxtGroupName.Clear();
                RefreshAll();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            if (GridGroups.SelectedItem is not Group group) return;

            if (group.Students.Count > 0)
            {
                MessageBox.Show("Спочатку видаліть або перемістіть усіх студентів групи.",
                    "Неможливо видалити", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _groups.Remove(group);
            PanelGroupInfo.Visibility = Visibility.Collapsed;
            TxtGroupTitle.Text = "← Оберіть групу зі списку";
            RefreshAll();
        }

        private void UpdateGroup_Click(object sender, RoutedEventArgs e)
        {
            if (GridGroups.SelectedItem is not Group group) return;

            try
            {
                group.Name = TxtGroupName.Text;
                TxtGroupTitle.Text = group.Name;
                RefreshAll();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GridGroups_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (GridGroups.SelectedItem is not Group group)
            {
                PanelGroupInfo.Visibility = Visibility.Collapsed;
                return;
            }

            TxtGroupName.Text  = group.Name;
            TxtGroupTitle.Text = $"Група: {group.Name}";

            TxtGrpCount.Text   = group.StudentCount.ToString();
            TxtGrpAvg.Text     = group.GroupAverage.ToString();
            TxtGrpPassing.Text = group.Students.Count(s => s.IsPassing).ToString();

            PanelGroupInfo.Visibility       = Visibility.Visible;
            GridGroupStudents.ItemsSource   = group.Students;
        }

        private void AddTeacher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var subject = CmbTSubject.SelectedItem as Subject;
                var teacher = new Teacher
                {
                    Id        = _nextTeacherId++,
                    FirstName = TxtTFirstName.Text,
                    LastName  = TxtTLastName.Text,
                    Subject   = subject
                };

                if (subject is not null)
                    subject.Teacher = teacher;

                _teachers.Add(teacher);
                RefreshAll();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (GridTeachers.SelectedItem is not Teacher teacher) return;

            if (teacher.Subject is not null)
                teacher.Subject.Teacher = null;

            _teachers.Remove(teacher);
            RefreshAll();
        }

        private void AddSubject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var subject = new Subject { Id = _nextSubjectId++, Name = TxtSubjectName.Text };
                _subjects.Add(subject);
                TxtSubjectName.Clear();
                RefreshAll();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteSubject_Click(object sender, RoutedEventArgs e)
        {
            if (GridSubjects.SelectedItem is not Subject subject) return;

            if (subject.Teacher is not null)
                subject.Teacher.Subject = null;

            _subjects.Remove(subject);
            RefreshAll();
        }

        private void CmbGStudent_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (CmbGStudent.SelectedItem is Student student)
                GridGrades.ItemsSource = student.Grades;
        }

        private void AddGrade_Click(object sender, RoutedEventArgs e)
        {
            if (CmbGStudent.SelectedItem is not Student student ||
                CmbGSubject.SelectedItem is not Subject subject) return;

            if (!double.TryParse(TxtGradeVal.Text, out double value))
            {
                MessageBox.Show("Введіть числове значення оцінки.", "Помилка введення",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var existing = student.Grades.FirstOrDefault(g => g.Subject == subject);
                if (existing is not null)
                    existing.Value = value;
                else
                    student.Grades.Add(new Grade { Subject = subject, Value = value });

                GridGrades.ItemsSource = null;
                GridGrades.ItemsSource = student.Grades;
                RefreshAll();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Помилка введення", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteGrade_Click(object sender, RoutedEventArgs e)
        {
            if (CmbGStudent.SelectedItem is not Student student ||
                GridGrades.SelectedItem is not Grade grade) return;

            student.Grades.Remove(grade);
            GridGrades.ItemsSource = null;
            GridGrades.ItemsSource = student.Grades;
            RefreshAll();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var results = _students.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(TxtSearchName.Text))
            {
                var query = TxtSearchName.Text.Trim().ToLowerInvariant();
                results = results.Where(s =>
                    s.FirstName.ToLowerInvariant().Contains(query) ||
                    s.LastName.ToLowerInvariant().Contains(query));
            }

            if (CmbSearchGroup.SelectedItem is Group selectedGroup)
                results = results.Where(s => s.Group == selectedGroup);

            if (double.TryParse(TxtSearchMin.Text, out double minGrade))
                results = results.Where(s => s.AverageGrade >= minGrade);

            if (double.TryParse(TxtSearchMax.Text, out double maxGrade))
                results = results.Where(s => s.AverageGrade <= maxGrade);

            if (CmbSearchStatus.SelectedItem is ComboBoxItem statusItem)
            {
                results = statusItem.Content.ToString() switch
                {
                    "Тільки успішні"   => results.Where(s => s.IsPassing),
                    "Тільки неуспішні" => results.Where(s => !s.IsPassing),
                    _                  => results
                };
            }

            if (CmbSearchSubject.SelectedItem is Subject filterSubject &&
                CmbSearchSubjectStatus.SelectedItem is ComboBoxItem subjStatusItem)
            {
                results = subjStatusItem.Content.ToString() switch
                {
                    "Склав предмет"    => results.Where(s => s.IsPassingSubject(filterSubject)),
                    "Не склав предмет" => results.Where(s => !s.IsPassingSubject(filterSubject)),
                    _                  => results
                };
            }

            var list = results.ToList();
            GridSearch.ItemsSource   = list;
            TxtSearchCount.Text      = $"Знайдено: {list.Count}";
            PanelSearchDetail.Visibility = Visibility.Collapsed;
        }

        private void SearchReset_Click(object sender, RoutedEventArgs e)
        {
            TxtSearchName.Clear();
            TxtSearchMin.Clear();
            TxtSearchMax.Clear();
            CmbSearchGroup.SelectedItem         = null;
            CmbSearchStatus.SelectedIndex       = 0;
            CmbSearchSubject.SelectedItem       = null;
            CmbSearchSubjectStatus.SelectedIndex = 0;
            GridSearch.ItemsSource              = _students;
            TxtSearchCount.Text                 = string.Empty;
            PanelSearchDetail.Visibility        = Visibility.Collapsed;
        }

        private void GridSearch_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (GridSearch.SelectedItem is not Student student)
            {
                PanelSearchDetail.Visibility = Visibility.Collapsed;
                return;
            }

            TxtSearchDetailName.Text   = student.FullName;
            TxtSearchDetailAvg.Text    = $"Середній бал: {student.AverageGrade}";
            TxtSearchDetailStatus.Text = student.IsPassing ? "✓ Успішний" : "✗ Неуспішний";
            ListSearchGrades.ItemsSource = student.Grades;
            PanelSearchDetail.Visibility = Visibility.Visible;
        }
    }
}
