import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import AddIcon from '@material-ui/icons/Add';
import { Button, Grid } from '@material-ui/core';
import { 
  getBranchLocationArraySelector, getBranchLocationsActiveDirectionSelector, getBranchLocationsActiveKeySelector,
  getTotalBranchLocationsNumberSelector, isBranchLocationsLoadingSelector, getBranchLocationsCurrentPageSelector
} from '../../../../selectors/branch-locations';
import ScreendoxTable, { EScreendoxTableType } from '../../../UI/table';
import { changeActiveSortObjectAction,changeCurrentPageRequest, createBranchLocationDescription, createBranchLocationName, createBranchLocationScreenProfile, getBranchLocationByIdRequest, setBranchLocationDisabled, setNewBranchLocationLoading } from '../../../../actions/branch-locations';
import { ButtonTextStyle, ContentContainer, TitleText } from '../../styledComponents';
import classes from  '../../pages.module.scss';
import ScreendoxModal from '../../../UI/modal';
import { closeModalWindow, EModalWindowKeys, openModalWindow } from '../../../../actions/settings';
import { getBranchLocationsRequest, IBranchLocationItemResponse } from 'actions/branch-locations';
import ScreendoxEditButton from '../../../UI/custom-buttons/editButton';
import AddNewBranchLocation from '../add-new-branch-location';
import AddNewBranchLocationActions from '../add-new-branch-location-actions';
import EditBranchLocationsActions from '../edit-branch-location-actions';
import { useHistory } from 'react-router-dom';
import { getToken  } from 'helpers/auth';
import CustomAlert from 'components/UI/alert';


const BranchLocationsList = (): React.ReactElement => {

  const dispatch = useDispatch();
  const history = useHistory();
  const branchLocationsList: Array<IBranchLocationItemResponse> = useSelector(getBranchLocationArraySelector);
  const isLoading: boolean = useSelector(isBranchLocationsLoadingSelector);
  const totalItems: number = useSelector(getTotalBranchLocationsNumberSelector);
  const sortKey = useSelector(getBranchLocationsActiveKeySelector);
  const currentPage = useSelector(getBranchLocationsCurrentPageSelector);
  const sortDirection = useSelector(getBranchLocationsActiveDirectionSelector);

  React.useEffect(() => {
    if(history.location.search !== '') {
      const branckLocationId = parseInt(history.location.search.replace('?', ''));
      dispatch(getBranchLocationByIdRequest(branckLocationId));
    }
    dispatch(getBranchLocationsRequest());
  }, [dispatch]);

  return (
      <ContentContainer>
        <CustomAlert />
        <Grid container justifyContent="flex-start" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
          <Grid item xs={6} style={{ textAlign: 'start' }}>
            <TitleText>
                Branch Locations
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
                dispatch(setBranchLocationDisabled(false))
                dispatch(openModalWindow(EModalWindowKeys.branchLocationsAddNewBranchLocation));
              }}
              >
                <ButtonTextStyle>
                  Add New Branch Location
                </ButtonTextStyle>
            </Button>
          </Grid>
        </Grid>
        <ScreendoxTable
          isLoading={isLoading}
          type={EScreendoxTableType.screenList}
          total={totalItems}
          isCollapsable="NO"
          onPageClick={(page: number) => {
            dispatch(changeCurrentPageRequest(page));
          }}
          currentPage={currentPage}
          isAutoRefreshDisabled={true}
          isPagination={true}
          selectedItemValue={null}
          onSelectHandler={(v: number) => {}}
          onTriggerAutoUpdate={() => {
            dispatch(getBranchLocationsRequest())
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
                name: 'Screen Profile / Department',
                key: 'ScreeningProfileName',
                action: changeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Action',
                key: 'Action',
              },
            ]
          }
          rows={branchLocationsList.map((r: IBranchLocationItemResponse) => {
            return { 
                Name: r.Name,
                Description: r.Description,
                ScreeningProfileName: r.ScreeningProfileName,
                Action: (
                  <ScreendoxEditButton
                    onClickHandler={e => {
                      e.stopPropagation();
                      if (r.BranchLocationID) {
                        history.push('?'+r.BranchLocationID)
                        dispatch(getBranchLocationByIdRequest(r.BranchLocationID));
                      }
                    }}
                  />
                ),
              onSelectItem: () => {},
              customStyleObject: {
                Name: {
                  width: '30%',
                },
                Description: {
                  width: '30%',
                },
                ScreeningProfileName: {
                  width: '30%',
                },
                Action: {
                  width: '10%',
                }
              },
              subItem: [],
              // disabled: r.Disabled
              }
          })}
        />
        <ScreendoxModal
          uniqueKey={EModalWindowKeys.branchLocationsAddNewBranchLocation}
          content={<AddNewBranchLocation />}
          actions={<AddNewBranchLocationActions />}
          title="Add New Branch Location"
          onConfirm={() => {
            dispatch(setNewBranchLocationLoading(false));
            dispatch(closeModalWindow(EModalWindowKeys.branchLocationsAddNewBranchLocation));
            dispatch(createBranchLocationName(''));
            dispatch(createBranchLocationScreenProfile(0));
            dispatch(createBranchLocationDescription(''))
          }}
        />
        <ScreendoxModal
          uniqueKey={EModalWindowKeys.branchLocationsEditBranchLocation}
          content={<AddNewBranchLocation />}
          actions={<EditBranchLocationsActions />}
          title="Edit Branch Location"
          onConfirm={() => {
            dispatch(closeModalWindow(EModalWindowKeys.branchLocationsEditBranchLocation));
            dispatch(createBranchLocationName(''));
            dispatch(createBranchLocationScreenProfile(0));
            dispatch(createBranchLocationDescription(''))
          }}
        />
      </ContentContainer>
  )
}

export default BranchLocationsList;
