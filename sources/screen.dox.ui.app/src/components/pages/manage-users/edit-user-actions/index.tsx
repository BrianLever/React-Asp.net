import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid , Button, TextField, Select, FormControl  } from '@material-ui/core';
import { ERouterUrls } from '../../../../router';
import { useHistory } from 'react-router';
import { ManageUserTextArea } from '../../styledComponents';
import { ButtonText } from '../../../UI/side-search-layout/styledComponents'
import { manageUsersSelectedUserIdSelector, manageUsersUserSelector } from 'selectors/manage-users';
import { manageUsersBlockRequest, manageUsersUpdateRequest, manageUsersDeleteRequest, manageUsersUnBlockRequest  } from 'actions/manage-users';

const EditUserActions = (): React.ReactElement => {

  const dispatch = useDispatch();
  const userId = useSelector(manageUsersSelectedUserIdSelector);
  const user = useSelector(manageUsersUserSelector);  

  React.useEffect(() => {

  }, [])

  return (
      <div className={'edit-user-action'} style={{ fontSize: 16, width: '100%' }}>
          <Button 
                size="large" 
                fullWidth 
                disabled={false}
                variant="contained" 
                color="primary" 
                style={{ backgroundColor: '#2e2e42', marginBottom: 10 }}
                onClick={() => {
                    if(userId) {
                        dispatch(manageUsersUpdateRequest(userId));
                    }
                }}
          >
            <ButtonText>Save Changes</ButtonText>
          </Button>
          <Button 
                size="large" 
                fullWidth 
                disabled={false}
                variant="contained" 
                color="primary" 
                style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42', marginBottom: 10}}
                onClick={() => { 
                   if(userId) {
                       dispatch(manageUsersDeleteRequest(userId));
                   }
                }}
          >
            <ButtonText style={{ color: '#2e2e42' }}>Delete</ButtonText>
          </Button>
          {!user.IsBlock?
            <Button 
            size="large" 
            fullWidth 
            disabled={false}
            variant="contained" 
            color="primary" 
            style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42'}}
            onClick={() => {
                if(userId) {
                    dispatch(manageUsersBlockRequest(userId))
                }
            }}
            >
                <ButtonText style={{ color: '#2e2e42' }}>Block</ButtonText>
            </Button>:
            <Button 
            size="large" 
            fullWidth 
            disabled={false}
            variant="contained" 
            color="primary" 
            style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42'}}
            onClick={() => {
                if(userId) {
                    dispatch(manageUsersUnBlockRequest(userId))
                }
            }}
            >
                <ButtonText style={{ color: '#2e2e42' }}>UnBlock</ButtonText>
            </Button>
          }  
      </div>
  )
}

export default EditUserActions;
