import { DepartmentCreateDialog } from "@/features/departments/department-create-dialog";
import { DepartmentViewVariant } from "@/features/departments/view/department-view-variant";
import { ListLayout } from "@/shared/components/ui/list-layout";
import { DepartmentView } from "@/widgets/departments/view/department-view";
import { DepartmentViewFilters } from "@/widgets/departments/view/department-view-filters";
import { DepartmentViewSearch } from "@/widgets/departments/view/department-view-search";

const { Container, Content, Header } = ListLayout;

export const DepartmentsPage = () => {
  return (
    <ListLayout className="p-2">
      <Header>
        <DepartmentViewSearch />
        <div className="flex gap-2 ml-auto">
          <DepartmentCreateDialog />
          <DepartmentViewVariant />
        </div>
      </Header>
      <Container>
        <Content>
          <DepartmentView />
        </Content>
        <DepartmentViewFilters />
      </Container>
    </ListLayout>
  );
};
