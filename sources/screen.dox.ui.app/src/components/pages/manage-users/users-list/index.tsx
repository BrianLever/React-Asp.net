import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid , Button } from '@material-ui/core';
import ScreendoxTable, { EScreendoxTableType } from '../../../UI/table';
import { ERouterUrls } from '../../../../router';
import { ContentContainer, TitleText , ButtonTextStyle } from '../../styledComponents';
import { getManageUsersListRequest, IManageUsersResponseItem, manageUsersChangeActiveSortObjectAction, manageUsersDetailRequest, setManageUsersCurrentPage, setManageUsersOrderDirection } from 'actions/manage-users';
import { getManageUserListSelector, manageUsersTotalSelector, isManageUserListLoading, manageUsersCurrentPageSelector, manageUsersOrderDirectionSelector , manageUsersOrderKeySelector } from 'selectors/manage-users';
import ScreendoxEditButton from '../../../UI/custom-buttons/editButton';
import { useHistory } from 'react-router';
import { ManageUsersModal } from 'components/UI/modal';
import { closeModalWindow, EModalWindowKeys, openModalWindow } from 'actions/settings';
import CreateUser from '../create-user';
import CreateUserActions from '../create-user-actions';
import EditUser from '../edit-user';
import EditUserActions from '../edit-user-actions';
import CustomAlert from 'components/UI/alert';

const UsersList = (): React.ReactElement => {

  const dispatch = useDispatch();
  const users: IManageUsersResponseItem[] = useSelector(getManageUserListSelector);
  const totals: number = useSelector(manageUsersTotalSelector);
  const isLoading: boolean = useSelector(isManageUserListLoading);
  const currentPage: number = useSelector(manageUsersCurrentPageSelector);
  const sortKey = useSelector(manageUsersOrderKeySelector);
  const sortDirection = useSelector(manageUsersOrderDirectionSelector);
  const history = useHistory();

  React.useEffect(() => {
    if(history.location.search !== '') {
      const userId = parseInt(history.location.search.replace('?', ''));
      dispatch(manageUsersDetailRequest(Number(userId)))
    }
  }, [])

  return (
      <ContentContainer>
        <CustomAlert />
        <Grid container  alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
          <Grid item xs={8}>
            <TitleText>
                Manage Users
            </TitleText>
          </Grid>
          <Grid item sm={4} style={{ textAlign: 'right'}}>  
                <Button 
                    size="large"  
                    disabled={false}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: 'rgb(46,46,66)' }}
                    onClick={() => {
                        dispatch(openModalWindow(EModalWindowKeys.manageUsersAddUser));
                    }}
                >
                    <ButtonTextStyle>
                        + Add New User
                    </ButtonTextStyle>
                </Button>
          </Grid>
        </Grid>
        <ScreendoxTable
          isLoading={isLoading}
          type={EScreendoxTableType.screenList}
          total={totals}
          onPageClick={(page: number) => {
              dispatch(setManageUsersCurrentPage(page));
              dispatch(getManageUsersListRequest());
          }}
          currentPage={currentPage}
          isAutoRefresh={false}
          isAutoRefreshDisabled={true}
          isPagination={true}
          selectedItemValue={null}
          onSelectHandler={(v: number) => {
            
          }}
          onTriggerAutoUpdate={() => {
            
          }}
          isCollapsable="NO"
          headers={
            [
              { 
                name: 'First Name',
                key: 'FirstName',
                action: manageUsersChangeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Last Name',
                key: 'LastName',
                action: manageUsersChangeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Username',
                key: 'Username',
                action: manageUsersChangeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Email',
                key: '',
              },
              { 
                name:  'Contact Phone',
                key: '',
              },
              { 
                name:  'Branch Location',
                key: 'BranchLocationName',
                action: manageUsersChangeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Action',
                key: '',
              },
            ]
          }
          rows={users.map((r: IManageUsersResponseItem) => {
            return { 
              FirstName: r.FirstName,
              LastName: r.LastName,
              Username: r.UserName,
              Email: r.Email,
              ContactPhone: r.ContactPhone,
              BranchLocation: r.BranchLocationName,
              Action: (
                <ScreendoxEditButton 
                  onClickHandler={e => {
                    e.stopPropagation();
                    if (r.UserID) {
                      history.push('?'+r.UserID);
                      dispatch(manageUsersDetailRequest(r.UserID))
                    }
                  }}
                />
              ),
              onSelectItem: () => {},
              customStyleObject: {
                
              },
              subItem: []
              }
          })}
        />
        <ManageUsersModal
          uniqueKey={EModalWindowKeys.manageUsersAddUser}
          content={<CreateUser />}
          actions={<CreateUserActions />}
          title="Create New User"
          onConfirm={() => {
            dispatch(closeModalWindow(EModalWindowKeys.manageUsersAddUser))
          }}
        />
        <ManageUsersModal
          uniqueKey={EModalWindowKeys.manageUsersEditUser}
          content={<EditUser />}
          actions={<EditUserActions />}
          title="Edit User Account"
          onConfirm={() => {
            dispatch(closeModalWindow(EModalWindowKeys.manageUsersEditUser))
          }}
        />
      </ContentContainer>
  )
}

export default UsersList;
