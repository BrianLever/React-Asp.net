import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import AddIcon from '@material-ui/icons/Add';
import { Button, Grid } from '@material-ui/core';
import { ContentContainer, TitleText, ButtonTextStyle } from '../../styledComponents';
import classes from  '../../pages.module.scss';
import { useHistory } from 'react-router-dom';
import { isScreenProfilesListRequestLoadingSelector, screenProfilesListSelector, screenProfileSortKeySelector,  screenProfileSortDirectionelector, screenProfilesTotalSelector, screenProfilesCurrentPageSelector } from 'selectors/screen-profiles';
import { getScreenProfileListRequest, IScreenProfilesResponseItem, changeCurrentPageRequest, changeActiveSortObjectAction, getScreenProfileByIdRequest,setCreateScreenProfileName, setNewScreenProfileLoading, setCreateScreenProfileDescription,  } from 'actions/screen-profiles';
import ScreendoxEditButton from '../../../UI/custom-buttons/editButton';
import ScreendoxTable, { EScreendoxTableType } from '../../../UI/table';
import ScreendoxModal, { ScreenProfileEditModal } from '../../../UI/modal';
import { closeModalWindow, EModalWindowKeys, openModalWindow } from '../../../../actions/settings';
import AddScreenProfile from '../add-screen-profile';
import EditScreenProfile from '../edit-screen-profile';
import AddScreenProfileActions from '../add-screen-profile-actions';
import EditScreenProfileActions from '../edit-screen-profile-actions';
import CustomAlert from 'components/UI/alert';

const ScreenProfilesList = (): React.ReactElement => {

  const dispatch = useDispatch();
  const history = useHistory();
  const isScreenProfileLoading = useSelector(isScreenProfilesListRequestLoadingSelector);
  const screenProfilesList = useSelector(screenProfilesListSelector);
  const sortKey = useSelector(screenProfileSortKeySelector);
  const sortDirection = useSelector(screenProfileSortDirectionelector);
  const totalScreenProfiles = useSelector(screenProfilesTotalSelector);
  const currentPage = useSelector(screenProfilesCurrentPageSelector);

  React.useEffect(() => {
    if(history.location.search !== '') {
      const screenProfileId = parseInt(history.location.search.replace('?', ''));
      dispatch(getScreenProfileByIdRequest(screenProfileId));
    }
    dispatch(getScreenProfileListRequest())
  }, [])

  return (
      <ContentContainer>
        <CustomAlert />
        <Grid container justifyContent="flex-start" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
          <Grid item xs={6} style={{ textAlign: 'start' }}>
            <TitleText>
                Screen Profiles
            </TitleText>
          </Grid>
          <Grid item xs={6} style={{ textAlign: 'end' }}>
            <Button startIcon={<AddIcon />} 
              size="large"  
              disabled={false}
              className={classes.removeBoxShadow}
              variant="contained" 
              color="primary" 
              style={{ backgroundColor: '#2e2e42' }}
              onClick={() => {
                dispatch(openModalWindow(EModalWindowKeys.screenProfileAdd));
              }}
              >
                <ButtonTextStyle>Add New Screen Profile</ButtonTextStyle>
            </Button>
          </Grid>
        </Grid>
        <ScreendoxTable
          isLoading={isScreenProfileLoading}
          type={EScreendoxTableType.screenList}
          total={totalScreenProfiles}
          isCollapsable="NO"
          onPageClick={(page: number) => {
            dispatch(changeCurrentPageRequest(page));
          }}
          currentPage={currentPage}
          isAutoRefreshDisabled={true}
          isPagination={true}
          selectedItemValue={null}
          onSelectHandler={ () => console.log('Screendox.')}
          onTriggerAutoUpdate={() => {
            // dispatch(getBranchLocationsRequest())
          }}
          headers={
            [
              { 
                name: 'Name',
                key: 'Name',
                action: changeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Description',
                key: 'Description',
                action: changeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Detail',
                key: 'Detail',
              },
            ]
          }
          rows={screenProfilesList.map((r: IScreenProfilesResponseItem) => {
            return { 
                id: r.ID,
                Name: r.Name,
                Description: r.Description,
                Action: (
                  <ScreendoxEditButton
                    onClickHandler={e => {
                      e.stopPropagation();
                      if (r.ID) {
                        history.push('?'+r.ID)
                        dispatch(getScreenProfileByIdRequest(r.ID));
                      }
                    }}
                  />
                ),
              onSelectItem: () => { console.log('Screendox.') },
              customStyleObject: {
                Name: {
                  width: '30%',
                },
                Description: {
                  width: '30%',
                },
                Action: {
                  width: '10%',
                }
              },
              subItem: [],
              }
          })}
        />      
        <ScreendoxModal
          uniqueKey={EModalWindowKeys.screenProfileAdd}
          content={<AddScreenProfile />}
          actions={<AddScreenProfileActions />}
          title="Add New Screen Profile"
          onConfirm={() => {
            dispatch(closeModalWindow(EModalWindowKeys.screenProfileAdd));
          }}
        />
        <ScreenProfileEditModal
          uniqueKey={EModalWindowKeys.screenProfileEdit}
          content={<EditScreenProfile />}
          actions={<EditScreenProfileActions />}
          title="Edit Profile Settings"
          onConfirm={() => {
            dispatch(closeModalWindow(EModalWindowKeys.screenProfileEdit));
            dispatch(setCreateScreenProfileDescription(''));
            dispatch(setCreateScreenProfileName(''))
            dispatch(setNewScreenProfileLoading(false));
          }}
        />
      </ContentContainer>
  )
}

export default ScreenProfilesList;
