import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import * as fileDownload from 'js-file-download';
import { 
    Button, FormControl, Grid, InputLabel, Select, TextField, 
} from '@material-ui/core';
import { TitleText, TitleTextH2,ContainerBlock } from './styledComponents';
import { getCssrsListRequest } from 'actions/c-ssrs-list';
import { ERouterUrls } from 'router';
import { useHistory } from 'react-router';
import { ButtonText } from '../styledComponents';
import { useParams } from 'react-router-dom';
import { cssrsReportUpdateRequest } from 'actions/c-ssrs-list/c-ssrs-report';

const CssrsRecentReportTemplate = (): React.ReactElement => {

    const dispatch = useDispatch();
    const history = useHistory();
    const { reportId } = useParams<{ reportId: string }>();

    return (
        <ContainerBlock>
            <Grid container spacing={1} >
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth 
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42' }}
                        onClick={() =>{
                            if(reportId) {
                                dispatch(cssrsReportUpdateRequest(Number(reportId)))
                            }
                        }}
                    >
                        <ButtonText>
                            Save Change
                        </ButtonText>
                    </Button>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth
                        variant="contained" 
                        color="default" 
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                        onClick={() => {
                            history.goBack();
                        } }
                    >
                        
                        <ButtonText style={{ color: '#2e2e42'}}>
                            Cancel
                        </ButtonText>
                    </Button>
                </Grid>

                <Grid item xs={12} style={{ textAlign: 'center',  marginTop: 30 }}>
                    <Button 
                        size="large" 
                        fullWidth 
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42' }}
                        onClick={() =>{}
                          
                        }
                    >
                        <ButtonText>Print</ButtonText>
                    </Button>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42' }}
                        onClick={() => {
                          history.push(ERouterUrls.CSSRS_LIST)
                        }}
                    >
                        <ButtonText>Return to C-SSRS List</ButtonText>
                    </Button>
                </Grid>
            </Grid>
        </ContainerBlock>
    )
}

export default CssrsRecentReportTemplate;
