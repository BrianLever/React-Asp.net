import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid , Button, TextField, Select, FormControl  } from '@material-ui/core';
import { ERouterUrls } from '../../../../router';
import { useHistory } from 'react-router';
import { ManageUserTextArea } from '../../styledComponents';
import { ButtonText } from '../../../UI/side-search-layout/styledComponents'
import { closeModalWindow, EModalWindowKeys } from 'actions/settings';
import { manageUsersCreateRequest } from 'actions/manage-users';


const CreateUserActions = (): React.ReactElement => {

  const dispatch = useDispatch();

  React.useEffect(() => {

  }, [])

  return (
      <div className={'create-user-action'} style={{ fontSize: 16, width: '100%' }}>
          <Button 
                size="large" 
                fullWidth 
                disabled={false}
                variant="contained" 
                color="primary" 
                style={{ backgroundColor: '#2e2e42', marginBottom: 10 }}
                onClick={() => {
                    dispatch(manageUsersCreateRequest());
                }}
          >
            <ButtonText>Add User</ButtonText>
          </Button>
          <Button 
                size="large" 
                fullWidth 
                disabled={false}
                variant="contained" 
                color="primary" 
                style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42'}}
                onClick={() => {
                    dispatch(closeModalWindow(EModalWindowKeys.manageUsersAddUser));
                }}
          >
            <ButtonText style={{ color: '#2e2e42' }}>Cancel</ButtonText>
          </Button>
      </div>
  )
}

export default CreateUserActions;
