using AutoMapper;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Services.Utilities;
using ProgrammersBlog.Shared.Utilities.Results.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Services.Concrete
{
    public class CategoryManager : ManagerBase,ICategoryService
    {
      
        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper):base(unitOfWork,mapper)
        {

        }
        

        public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName)
        {
            var category = Mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;
            var addedCategory=await UnitOfWork.Categories.AddAsync(category);
            await UnitOfWork.SaveAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Add(addedCategory.Name),new CategoryDto
            {
                Category=addedCategory,
                ResultStatus=ResultStatus.Success,
                Message = Messages.Category.Add(addedCategory.Name)
            });

        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var categoriesCount=await UnitOfWork.Categories.CountAsync();
            if (categoriesCount>-1)
            {
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Success, "Beklenmeyen Bir Hata Oluştu",-1);
            }
        }

        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync(c=>!c.IsDeleted);
            if (categoriesCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Success, "Beklenmeyen Bir Hata Oluştu", -1);
            }
        }

        public async Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName)
        {

            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                category.IsActive = false;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                var deletedCategory = await UnitOfWork.Categories.UpdateAsync(category);
                await UnitOfWork.SaveAsync();
                return new DataResult<CategoryDto>(ResultStatus.Success,Messages.Category.Delete(deletedCategory.Name), new CategoryDto
                {
                    Category = deletedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Category.Delete(deletedCategory.Name)
                });

            }

            return new DataResult<CategoryDto>(ResultStatus.Error,Messages.Category.NotFound(false), new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFound(false)
            });
        }

        public async Task<IDataResult<CategoryDto>> GetAsync(int id)
        {
            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == id, c => c.Articles);
            if (category != null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto { Category = category, ResultStatus = ResultStatus.Success });
            }

            return new DataResult<CategoryDto>(ResultStatus.Error, Messages.Category.NotFound(false), data: new CategoryDto { Category=null,ResultStatus=ResultStatus.Error,Message= Messages.Category.NotFound(false) });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(null, c => c.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto { Categories = categories, ResultStatus = ResultStatus.Success  });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(true), new CategoryListDto { Categories=categories,ResultStatus=ResultStatus.Error,Message= Messages.Category.NotFound(true) });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(c => !c.IsDeleted, c => c.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto { Categories = categories, ResultStatus = ResultStatus.Success });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(true), new CategoryListDto { Categories = categories, ResultStatus = ResultStatus.Error, Message = Messages.Category.NotFound(true) });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(c => !c.IsDeleted && c.IsActive, c => c.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto { Categories = categories, ResultStatus = ResultStatus.Success });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(true), null);
        }

        public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId)
        {
            var result = await UnitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
            if (result)
            {
                var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
                var categoryUpdateDto = Mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);

            }
            else
            {
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, Messages.Category.NotFound(false),null);
            }
        }

        public async Task<IResult> HardDeleteAsync(int categoryId)
        {
            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                await UnitOfWork.Categories.DeleteAsync(category);
                await UnitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, Messages.Category.HardDelete(category.Name));
            }
            return new Result(ResultStatus.Error, Messages.Category.NotFound(false));
        }

        public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var oldCategory = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);

            var category = Mapper.Map<CategoryUpdateDto,Category>(categoryUpdateDto,oldCategory);
            category.ModifiedByName = modifiedByName;
            var updatedCategory=await UnitOfWork.Categories.UpdateAsync(category);
            await UnitOfWork.SaveAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Update(updatedCategory.Name),new CategoryDto
            {
                Category=updatedCategory,
                ResultStatus=ResultStatus.Success,
                Message = Messages.Category.Update(updatedCategory.Name)
            });

        }

        public async Task<IDataResult<CategoryListDto>> GetAllByDeletedAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(c => c.IsDeleted);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto { Categories = categories, ResultStatus = ResultStatus.Success });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(true), null);
        }

        public async Task<IDataResult<CategoryDto>> UndoDeleteAsync(int categoryId, string modifiedByName)
        {
            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                category.IsDeleted = false;
                category.IsActive = true;
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                var deletedCategory = await UnitOfWork.Categories.UpdateAsync(category);
                await UnitOfWork.SaveAsync();
                return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.UndoDelete(deletedCategory.Name), new CategoryDto
                {
                    Category = deletedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Category.UndoDelete(deletedCategory.Name)
                });

            }

            return new DataResult<CategoryDto>(ResultStatus.Error, Messages.Category.NotFound(false), new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFound(false)
            });
        }
    }
}
