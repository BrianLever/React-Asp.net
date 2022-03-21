import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import { v4 as uuidv4 } from 'uuid';
import { IActionPayload } from 'actions';
import IconButton from '@material-ui/core/IconButton';
import CloseIcon from '@material-ui/icons/Close';
import { ColumdText, ColumdPaginationText, TableHeaderContainer, TableBodyContainer, TableHeaderRowInner, 
  TableHeaderColInner, TablebodyList, TableBodyRow, TableBodyRowButton, TableBodyRowButtonSpan, 
  IndicatorSpan, TableBodyColSpan, TableBodyTabPanel, TableBodyTabPanelContent, TableBodyAnchor, TableBodyAnchorContent, 
  TableBodyAnchorGraphic, TableBodyAnchorText, TableBodyCol, LoadingContainer
} from './styledComponents';
import SwitchComponent from '../switch';
import { ReportIcon, ReportText, ReportBlackIcon } from './styledComponents';
import CircularProgress from '@material-ui/core/CircularProgress';
import CSS from 'csstype';
import customClasss from  '../../pages/pages.module.scss';
import { Button, Grid } from '@material-ui/core';
import ScreendoxPagination from '../pagination';
import { cssrsReportCopyRequest } from 'actions/c-ssrs-list/c-ssrs-report';
import { ButtonText } from '../side-search-layout/styledComponents';
import { isReportCreateLoadingSelector } from 'selectors/c-ssrs-list/c-ssrs-report';

export enum EScreendoxTableType {
  screenList = 'SCREEN_LIST',
}

export type TTableRowRoute = {
  link: string;
  name: string;
}

export type TTableRow = {
  onClick?: () => void;
  customStyle?: CSS.Properties;
  customStyleObject?: { [k: string]: CSS.Properties };
  route?: TTableRowRoute;
  subItem?: TTableRow[]; 
  [k: string]: string | number | any | React.ReactElement;
}

export type TRowProps = {
  isFaild?: boolean;
  row: TTableRow;
  isLoading: boolean;
  selectedItem?: boolean; 
  onSelectHandler: () => void; 
  isCollapsable?: 'YES' | 'NO';
  disabled?:boolean;
}


export type THeader = {
  name: string;
  key: string;
  direction?: any;
  active?: string; 
  isSortable?: boolean;
  action?: (k: string, d: string) => IActionPayload;
}

export interface IScreendoxTableProps {
  isLoading?: boolean;
  isCollapsable?: 'YES' | 'NO';
  isFaild?: boolean;
  type: EScreendoxTableType;
  headers: Array<THeader>;
  rows: Array<TTableRow>;
  currentPage: number;
  isPagination: boolean;
  isAutoRefresh?: boolean;
  isAutoRefreshDisabled?: boolean;
  selectedItemValue: string | number | null;
  disabled?: boolean;
  total: number;
  onSelectHandler: (v: number) => void;
  onPageClick?: (page: number) => void;
  columnWidths?: string[];
  onTriggerAutoUpdate: () => void;
  cssrsTable?:boolean;
}

export const MAXIMUM_RECORDS_PER_PAGE = 20;


const ScreendoxCustomTable = (props: IScreendoxTableProps): React.ReactElement => {
  const [isShown, setIsShown] = React.useState(false);
  const [selectedId, setSelectedId] = React.useState<null | number>(null);
  const { 
    isCollapsable= 'YES', headers, isLoading, isFaild, rows, selectedItemValue,
    currentPage, total, onSelectHandler, onPageClick, onTriggerAutoUpdate, columnWidths, cssrsTable = false
  } = props;
  const isRows = rows && rows.length;
  const dispatch = useDispatch();
  const isCreatingLoading = useSelector(isReportCreateLoadingSelector);


  return (
    <Grid container>
      <div className={customClasss.table} style={{ width: '100%' }}>
        <TableHeaderContainer className={customClasss.tableHead}>         
          <TableHeaderRowInner>
              {
                headers.map((h: THeader, i: number) => (
                  <TableHeaderColInner
                    key={h.key} 
                    style={{ flexBasis: columnWidths && `calc(${columnWidths[i]} - 0rem)` }}
                  >
                    <p
                      onClick={e => {
                        e.stopPropagation();
                        if (h.key === h.active) {
                          const dir = h.direction === 'asc' ? 'desc' : 'asc';
                          h.action && dispatch(h.action(h.key, dir));
                        } else {
                          h.action && dispatch(h.action(h.key, 'asc'));
                        }
                        onSelectHandler(0);
                      }}
                    >
                      <ColumdText>{h.name}</ColumdText>
                    </p>
                  </TableHeaderColInner>                 
                ))
              }
          </TableHeaderRowInner>
        </TableHeaderContainer>
        <TableBodyContainer>
          <TableHeaderRowInner>
            <TableHeaderColInner>
                <TablebodyList>
                { isRows ? rows.map((row) => {
                  const selected = row.id === selectedItemValue;
                  const excludeFunc = (k: string) => k !== 'subItem' && k !== 'onSelectItem' && k !== 'id' && k !== 'customStyle' && k !== 'customStyleObject';
                  const outerCells = Object.keys(row).filter(excludeFunc).map((k: string) => row[k]);
                  const outerCellsKeys = Object.keys(row).filter(excludeFunc).map((k: string) => k);
                  const innerCells: Array<Array<string>> = [];
                  const innerCellKeys: Array<Array<string>> = [];
                  const links: any[] = [];
                  if (Array.isArray(row.subItem)) {
                    row.subItem?.forEach((dataObject, index: number) => {
                      return Object.keys(dataObject)
                      .filter((k: string) => {
                        if (!k || k === 'customStyle' || k === 'customStyleObject') {
                          return false;
                        }
                        if (k === 'route') {
                            links[index] = (dataObject[k] as TTableRowRoute);
                            return false;
                        }
                        return true;
                      })
                      .forEach(d => {
                        if (!innerCells[index]) {
                          innerCells[index] = [];
                          innerCellKeys[index] = [];
                        }
                        innerCells[index].push(dataObject[d])
                        innerCellKeys[index].push(d);
                      }); 
                    });
                  }
                  return (
                    <TableBodyRow >
                        <TableBodyRowButton
                          selected={selected}
                          onClick={e => {
                            e.stopPropagation();
                            if (!isLoading) {
                              onSelectHandler(row.id);
                              row.onSelectItem && row.onSelectItem()
                            }
                          }}
                        >
                          <TableBodyColSpan>
                          {
                          (isCollapsable === 'YES') ? (
                            <IndicatorSpan>
                                <IconButton 
                                  aria-label="expand row" 
                                  size="small"
                                  disabled={isLoading}
                                >
                                  <CloseIcon fontSize="small" style={{ color: '#2e2e42', fontWeight: 900, transform: selected?'translate3d(0,0,0) rotate(45deg)':'translate3d(0,0,0) rotate(90deg)', transitionDuration: '300ms'}} />
                                </IconButton> 
                            </IndicatorSpan>
                          ):null
                          }    
                          {outerCells && outerCells.map((k: string, i: number) => {
                            const unigueKey = uuidv4();
            
                            let cstyle: CSS.Properties = {};
                            let styleAssigned: boolean = false;
                            const columnKey = outerCellsKeys[i];
                         
                            if (row.customStyleObject) {
                              cstyle = row.customStyleObject[columnKey];
                              styleAssigned = true;
                            }
                            else if (!styleAssigned && row.customStyle) {
                              cstyle = row.customStyle;
                            }
                           
                          return(<div style={{ ...cstyle}} key={unigueKey}>
                              <p >{k}</p>
                          </div>)
                          })}
                          </TableBodyColSpan>
                        </TableBodyRowButton>
                        
                        <TableBodyTabPanel selected={selected} >
                          {
                            (!isFaild && ((isLoading && selected) || (Array.isArray(row.subItem) && !row.subItem.length && selected))) ? (
                              <LoadingContainer>
                                <Grid container justifyContent="center" spacing={2} style={{ marginTop: 45 }}>
                                  <CircularProgress disableShrink style={{ color: '#2e2e42' }}/>
                                </Grid>
                              </LoadingContainer>
                            ) :
                            (Array.isArray(row.subItem) && row.subItem.length && selected  && !isFaild) ? (
                            <TableBodyTabPanelContent>
                              {cssrsTable?
                              <TableHeaderRowInner style={{ justifyContent: 'left' }}>
                                <Button 
                                    size="large"  
                                    disabled={isCreatingLoading}
                                    variant="contained" 
                                    color="primary" 
                                    style={{ backgroundColor: '#2e2e42', minWidth: 150  }}
                                    onClick={() => { dispatch(cssrsReportCopyRequest(row.id))}}
                                >
                                    <ButtonText>
                                      Create C-SSRS Lifetime/Recent Report
                                    </ButtonText>
                                </Button>
                              </TableHeaderRowInner>:null}
                              {innerCells && innerCells.map((d: Array<string>, i: number) => {console.log(d)
                                const data = (Array.isArray(row.subItem) && row.subItem[i]) ?  row.subItem[i] : {}
                                const uuidFirst = uuidv4();
                                const uuid = uuidv4();
      
                                let cstyle: CSS.Properties =  {};
                                const linkColumnName = "route";
                                
                                let styleAssigned: boolean = false;
      
                                if (data.customStyleObject) {
                                  cstyle = data.customStyleObject[linkColumnName];
                                  styleAssigned = true;
                                }
                                else if (!styleAssigned && data.customStyle) {
                                  cstyle = data.customStyle;
                                }

                                const link = (
                                  <TableBodyCol key={uuidFirst} style={cstyle}
                                  >
                                    <TableBodyAnchor >
                                      <Link
                                          to={links[i].link}
                                          style={{ color: 'inherit', textDecoration: 'inherit',}}
                                        >
                                        <TableBodyAnchorContent
                                          onMouseEnter={(e) => { setSelectedId(i); setIsShown(true); }}
                                          onMouseLeave={() => setIsShown(false)}
                                        >
                                            <TableBodyAnchorGraphic>
                                              {(isShown && (i === selectedId))?<ReportBlackIcon />:<ReportIcon />} 
                                            </TableBodyAnchorGraphic>
                                            <TableBodyAnchorText>
                                                <ReportText style={{ fontWeight: 600 }} >
                                                  {links[i].name}
                                                </ReportText>
                                            </TableBodyAnchorText>
                                        </TableBodyAnchorContent>
                                      </Link>
                                  </TableBodyAnchor>
                                </TableBodyCol>
                                )

                                return (
                                  <TableHeaderRowInner key={uuid}>
                                      {
                                        d.map((k: string, index: number) => {
                                          let ccstyle: CSS.Properties =  {};
                                          const kIndex = innerCellKeys[i] &&  innerCellKeys[i][index];


                                        if (kIndex && data && data.customStyleObject && data.customStyleObject[kIndex]) {
                                          ccstyle = data.customStyleObject[kIndex];
                                        }
                                        
                                        return (
                                          <>
                                            { (index === 0) ? link : null }
                                            <TableBodyCol
                                              key={index} 
                                              style={{...ccstyle, fontSize: '1em', display: 'flex'}}
                                              
                                            >
                                              <TableBodyAnchorText>{k}</TableBodyAnchorText>
                                            </TableBodyCol>
                                          </>
                                        )
                                      })
                                    }
                                  </TableHeaderRowInner>
                                )
                              })}
                            </TableBodyTabPanelContent>):null
                          }
                        </TableBodyTabPanel>
                    </TableBodyRow>
                  )
                  }): null }
                </TablebodyList>
            </TableHeaderColInner>
          </TableHeaderRowInner>
        </TableBodyContainer>
      </div>
        {
          isRows ? (
          <Grid 
            container 
            justifyContent="center" 
            alignItems="center"
            style={{ 
              paddingTop: '10px', 
              paddingBottom: '10px', 
              overflow: 'hidden' 
            }} 
          >
            <Grid item xs={12} style={{ paddingBottom: '50px' }}>
              <Grid container justifyContent="center" alignItems="center" spacing={1}>
                <Grid item xs={3} />
                <Grid item xs={6} style={{ textAlign: 'center' }}>
                  <ColumdPaginationText> { currentPage } OF { total } </ColumdPaginationText>
                </Grid>
                <Grid item xs={3} style={{ textAlign: 'right' }}>
                  { props.isAutoRefreshDisabled ? 
                    null
                  : ( 
                    <SwitchComponent type="regular" defaultValue={props.isAutoRefresh} switchHandler={() => onTriggerAutoUpdate()} />
                    )
                  }
                </Grid>
              </Grid>
            </Grid>
            <Grid item xs={12}>
              <Grid container justifyContent="center" alignItems="center">
                    <ScreendoxPagination 
                      currentPage={currentPage}
                      count={total} 
                      disabled={isLoading}
                      defaultPage={1}
                      onChange={(event: React.ChangeEvent<unknown>, page: number) => {
                        event.stopPropagation();
                        onPageClick && onPageClick(page);
                        onSelectHandler(0);
                      }}
                    />
              </Grid>
            </Grid>
          </Grid>
        ) : null
      }
    </Grid>
  )
}

export default ScreendoxCustomTable;
