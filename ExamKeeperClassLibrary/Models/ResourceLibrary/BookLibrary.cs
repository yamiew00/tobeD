using ExamKeeperClassLibrary.Models.ResourceLibrary.Resources;
using ExamKeeperClassLibrary.Models.ResourceLibrary.SubjectMeta;
using ExamKeeperClassLibrary.Models.ResourceLibrary.YearBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExamKeeperClassLibrary.Models.ResourceLibrary
{
    public class BookLibrary
    {
        public Dictionary<string, ResourceBook> Books { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString">Configs["resourceLibrary_ConnectionString"]</param>
        /// <param name="definitionLibrary"></param>
        public BookLibrary(string connectionString, 
                       DefinitionLibrary definitionLibrary, 
                       SubjectMetaContext subjectMetaContext,
                       resourceContext resourceContext)
        {
            //context
            yearBooksContext yearBooksContext = new yearBooksContext(connectionString + "yearBooks");

            //先取回所有書(從BookList)
            Books = yearBooksContext.BookLists
                                    .ToDictionary(item => item.BookId,
                                                item => new ResourceBook()
                                                {
                                                    BookId = item.BookId,
                                                    VolumeName = item.BookId.Substring(item.BookId.Length-3, 3),
                                                    Version = definitionLibrary.Publisher[item.BookId[3..].Split("-")[0]],
                                                    Year = item.Year,
                                                    Curriculum = item.Curriculum,
                                                    EduSubject = item.EduSubject
                                                });

            //先一個一個寫，之後要改。
            Create109Tree(yearBooksContext, subjectMetaContext);
            Create110Tree(yearBooksContext, subjectMetaContext);
            Create111Tree(yearBooksContext, subjectMetaContext);

            //建構書本的樹狀結構
            foreach (var book in Books.Values)
            {
                foreach (var resourceChapter in book.Chapters.Values)
                {
                    if (resourceChapter.ParentUID == null)
                    {
                        book._ChapterTree.Add(resourceChapter);
                    }
                    else
                    {
                        var parent = book.Chapters[resourceChapter.ParentUID];
                        //要先組code 再AddChild
                        resourceChapter.Code = parent.Code + "-" + resourceChapter.Code;
                        parent.AddChild(resourceChapter);
                    }
                }
                book._ChapterTree = book._ChapterTree
                                        .OrderBy(tree => 
                                        {
                                            if(int.TryParse(tree.Code.Split("-")[0], out var result))
                                            {
                                                return result;
                                            }
                                            return 0;
                                        }).ToList();
            }
        }

        private void Create111Tree(yearBooksContext yearBooksContext, SubjectMetaContext subjectMetaContext)
        {
            var chapter111 = yearBooksContext.Chapter111s.AsEnumerable().ToList();

            var book111 = yearBooksContext.BookMeta111s.AsEnumerable();
            foreach (var bookMeta in book111)
            {
                var subject = bookMeta.EduSubject;
                var metaUID = bookMeta.MetaUid;
                var bookId = bookMeta.BookId;

                //科目
                var subjectMeta = subjectMetaContext.GetByUID(subject, metaUID);

                //章節
                var chapter = chapter111.FirstOrDefault(c => c.Uid == bookMeta.ParentUid);

                //寫回書本裡
                Books[bookId].AddKnowledge(chapter.Uid, new ResourceKnowledge()
                {
                    Code = subjectMeta.Code,
                    Name = subjectMeta.Name
                });
            }

            //走訪資料表[Chapter???]
            foreach (var chapter in chapter111)
            {
                //在書本中加節點
                Books[chapter.BookId].AddNode(new ResourceChapter()
                {
                    UID = chapter.Uid,
                    Name = chapter.Name,
                    Code = chapter.Code,
                    ParentUID = chapter.ParentUid
                });
            }
        }

        private void Create110Tree(yearBooksContext yearBooksContext, SubjectMetaContext subjectMetaContext)
        {
            var chapter110 = yearBooksContext.Chapter110s.AsEnumerable().ToList();

            var book110 = yearBooksContext.BookMeta110s.AsEnumerable();
            foreach (var bookMeta in book110)
            {
                var subject = bookMeta.EduSubject;
                var metaUID = bookMeta.MetaUid;
                var bookId = bookMeta.BookId;

                //科目
                var subjectMeta = subjectMetaContext.GetByUID(subject, metaUID);

                //章節
                var chapter = chapter110.FirstOrDefault(c => c.Uid == bookMeta.ParentUid);

                //寫回書本裡
                Books[bookId].AddKnowledge(chapter.Uid, new ResourceKnowledge()
                {
                    Code = subjectMeta.Code,
                    Name = subjectMeta.Name
                });
            }

            //走訪資料表[Chapter???]
            foreach (var chapter in chapter110)
            {
                //在書本中加節點
                Books[chapter.BookId].AddNode(new ResourceChapter()
                {
                    UID = chapter.Uid,
                    Name = chapter.Name,
                    Code = chapter.Code,
                    ParentUID = chapter.ParentUid
                });
            }
        }

        private void Create109Tree(yearBooksContext yearBooksContext, SubjectMetaContext subjectMetaContext)
        {
            var chapter109 = yearBooksContext.Chapter109s.AsEnumerable().ToList();

            var book109 = yearBooksContext.BookMeta109s.AsEnumerable();
            foreach (var bookMeta in book109)
            {
                var subject = bookMeta.EduSubject;
                var metaUID = bookMeta.MetaUid;
                var bookId = bookMeta.BookId;

                //科目
                var subjectMeta = subjectMetaContext.GetByUID(subject, metaUID);

                //章節
                var chapter = chapter109.FirstOrDefault(c => c.Uid == bookMeta.ParentUid);

                //寫回書本裡
                Books[bookId].AddKnowledge(chapter.Uid, new ResourceKnowledge()
                {
                    Code = subjectMeta.Code,
                    Name = subjectMeta.Name
                });
            }

            //走訪資料表[Chapter???]
            foreach (var chapter in chapter109)
            {
                //在書本中加節點
                Books[chapter.BookId].AddNode(new ResourceChapter()
                {
                    UID = chapter.Uid,
                    Name = chapter.Name,
                    Code = chapter.Code,
                    ParentUID = chapter.ParentUid
                });
            }
        }
    }
}
