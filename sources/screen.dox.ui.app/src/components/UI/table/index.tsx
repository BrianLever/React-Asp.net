import React from 'react';
import { useDispatch } from 'react-redux';
import { Link } from 'react-router-dom';
import { v4 as uuidv4 } from 'uuid';
import { IActionPayload } from 'actions';
import Collapse from '@material-ui/core/Collapse';
import IconButton from '@material-ui/core/IconButton';
import CloseIcon from '@material-ui/icons/Close';
import AddIcon from '@material-ui/icons/Add';
import {
  TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, TableSortLabel, Box
} from '@material-ui/core';
import { ColumdText, useRowStyles, ColumdPaginationText } from './styledComponents';
import Pagination, { usePagination } from '@material-ui/lab/Pagination';
import SwitchComponent from '../switch';
import { ReportIcon, ReportText, ReportBlackIcon } from './styledComponents';
import CircularProgress from '@material-ui/core/CircularProgress';
import CSS from 'csstype';
import customClasss from  '../../pages/pages.module.scss';
import { CssrsListPlusButton } from 'components/pages/styledComponents';
import ScreendoxPagination from 'components/UI/pagination'

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

const Row = (props: TRowProps) => {
  const [isShown, setIsShown] = React.useState(false);
  const [selectedId, setSelectedId] = React.useState<null | number>(null);
  const { 
    row, 
    isLoading, 
    selectedItem = false, 
    onSelectHandler, 
    isFaild = false,
    isCollapsable = 'YES',
    disabled
  } = props;

  const classes = useRowStyles();

  const excludeFunc = (k: string) => k !== 'subItem' && k !== 'onSelectItem' && k !== 'id' && k !== 'customStyle' && k !== 'customStyleObject';

  const outerCells = Object.keys(row).filter(excludeFunc).map((k: string) => row[k]);
  const outerCellsKeys = Object.keys(row).filter(excludeFunc).map((k: string) => k);

  const links: any[] = [];
  const innerCells: Array<Array<string>> = [];
  const innerCellKeys: Array<Array<string>> = [];
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
    <React.Fragment>   
      <TableRow
        className={`${classes.root} ${ selectedItem ? classes.rootSelected : '' }`}
        onClick={e => {
          e.stopPropagation();
          if (!isLoading) {
            onSelectHandler();
            row.onSelectItem && row.onSelectItem()
          }
        }}
      >
        {
          (isCollapsable === 'YES') ? (
            <TableCell
              style={{ width: '5%', fontSize: '1em' }}
              key="colapsable-column"
            >
              <div>
              <IconButton 
                aria-label="expand row" 
                size="small"
                disabled={isLoading}
              >
                {
                  selectedItem ? 
                    (<CssrsListPlusButton  style={{ color: '#2e2e42',transform:'rotate(45deg)' }} />) : 
                    (<CssrsListPlusButton  style={{ color: '#2e2e42'}} />) 
                }
              </IconButton> 
              </div>    
            </TableCell>            
          ) : null          
        }       
        {
          outerCells.map((k: string, i: number) => {
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

            return (
              <TableCell 
                key={unigueKey} 
                align="left" 
                size="small" 
                style={{ ...cstyle, textDecoration: disabled?'line-through':'none' , color: disabled?'gray':'none', fontSize: '1em'} }
              >
                {k}
              </TableCell>
            )
          })
        }
      </TableRow>
      <TableRow
        className={`${classes.innerRoot} ${ selectedItem ? classes.selectedInner : '' }`}
      > 
        <TableCell style={{ paddingBottom: 0, paddingTop: 0, paddingLeft:0, paddingRight:0, height: selectedItem ? '80px' : '0px', fontSize: '1em'  }} colSpan={outerCellsKeys.length + 1}>

          {
            (!isFaild && ((isLoading && selectedItem) || (Array.isArray(row.subItem) && !row.subItem.length && selectedItem))) ? (
              <Grid container justifyContent="center" spacing={2}>
                <CircularProgress disableShrink style={{ color: '#2e2e42' }}/>
              </Grid>
            ) :
           (Array.isArray(row.subItem) && row.subItem.length && selectedItem && !isFaild) ? (
              <Collapse in={selectedItem} timeout="auto" unmountOnExit >
                <Box margin={1} style={{ margin: 0 }}>
                  <Table aria-label="purchases">
                    <TableBody>                                                                                         
                      {
                        innerCells.map((d: Array<string>, i: number) => {
                          const data = (Array.isArray(row.subItem) && row.subItem[i]) ?  row.subItem[i] : {}
                          const uuidFirst = uuidv4();
                          const uuid = uuidv4();
                          const uuidLast = uuidv4();


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
                            <>
                              <TableCell 
                                style={{ width: '5%' }}
                                key={uuidFirst} 
                                align="left"
                              >
                                <Link
                                  to={links[i].link}
                                  style={{ color: 'inherit', textDecoration: 'inherit',}}
                                  onMouseEnter={(e) => { setSelectedId(i); setIsShown(true); }}
                                  onMouseLeave={() => setIsShown(false)}
                                >
                                  {(isShown && (i === selectedId))?<ReportBlackIcon />:<ReportIcon />} 
                                </Link>
                              </TableCell>
                              <TableCell 
                                style={cstyle}
                                key={uuidLast} 
                                align="left" 
                              >
                                <Link
                                  to={links[i].link}
                                  style={{ color: 'inherit', textDecoration: 'inherit'}}
                                  onMouseEnter={(e) => { setSelectedId(i); setIsShown(true) }}
                                  onMouseLeave={() => setIsShown(false)}
                                >
                                  <ReportText style={{ fontWeight: 600 }} >
                                    {links[i].name}
                                  </ReportText>
                                </Link>
                              </TableCell>
                            </>

                          )
                          return (
                            <TableRow key={uuid} className={classes.innerRoot}>
                              {
                                  d.filter(c => !!c).map((k: string, index: number) => {
                                    let ccstyle: CSS.Properties =  {};
                                    const kIndex = innerCellKeys[i] &&  innerCellKeys[i][index];


                                  if (kIndex && data && data.customStyleObject && data.customStyleObject[kIndex]) {
                                    ccstyle = data.customStyleObject[kIndex];
                                  }
                                  
                                  return (
                                    <>
                                      { (index === 0) ? link : null }
                                      <TableCell 
                                        key={index} 
                                        align="left" 
                                        style={{...ccstyle, fontSize: '1em'}}
                                        
                                      >
                                        {k}
                                      </TableCell>
                                    </>
                                  )
                                })
                              }
                            </TableRow>
                          )
                        })
                      }
                      </TableBody>
                  </Table>
                </Box>
              </Collapse>
            ) : null
          }
        </TableCell>
      </TableRow>
    </React.Fragment>
  );
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
  onTriggerAutoUpdate: () => void;
}

export const MAXIMUM_RECORDS_PER_PAGE = 20;

const ScreendoxTable = (props: IScreendoxTableProps): React.ReactElement => {

  const { 
    isCollapsable= 'YES', headers, isLoading, isFaild, rows, selectedItemValue,
    currentPage, total, onSelectHandler, onPageClick, onTriggerAutoUpdate
  } = props;
  const isRows = rows && rows.length;
  const dispatch = useDispatch();
  
  return (
    <TableContainer component={Paper} style={{
      boxShadow: 'none',
      // minWidth: '1000px'
    }}>
      <Table className={customClasss.table}>
        <TableHead className={customClasss.tableHead}>         
          <TableRow>
              { 
                isCollapsable === 'YES' ? (
                  <TableCell key="colapsbale-column" style={{ textAlign: 'left' }} />
                ) : null
              }
              {
                headers.map((h: THeader) => (
                  <TableCell
                    align={'left'}
                    key={h.key} 
                    sortDirection={(h.key === h.active) ? h.direction : 'desc'}
                  >
                    <TableSortLabel
                      disabled={isLoading}
                      active={(h.key === h.active)}
                      direction={(h.key === h.active) ? h.direction : 'desc'}
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
                    </TableSortLabel>
                  </TableCell>                 
                ))
              }
          </TableRow>
        </TableHead>
        <TableBody>
            { isRows ? rows.map((row) => {
              const selected = row.id === selectedItemValue;
              return (
                <Row 
                  key={row.id ? row.id : uuidv4()}
                  isLoading={isLoading || false}
                  isCollapsable={isCollapsable}
                  isFaild={isFaild}
                  row={row} 
                  selectedItem={selected} 
                  onSelectHandler={() => onSelectHandler(row.id)}
                  disabled={row.disabled}
                />
              )
            }): null }
        </TableBody>
      </Table>
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
                <Grid item xs={12} />
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                  <ColumdPaginationText> { currentPage } OF { total } </ColumdPaginationText>
                </Grid>
              </Grid>
            </Grid>
            <Grid item xs={12}>
              <Grid container justifyContent="center" alignItems="center">
              <Grid item xs={3} />
              <Grid item xs={6} >
                <Grid container justifyContent="center" alignItems="center" >
                  {/* <Pagination
                    page={currentPage}
                    count={total} 
                    shape="rounded"
                    showFirstButton 
                    showLastButton
                    disabled={isLoading}
                    defaultPage={1}
                    onChange={(event: React.ChangeEvent<unknown>, page: number) => {
                      event.stopPropagation();
                      onPageClick && onPageClick(page);
                      onSelectHandler(0);
                    }}
                  /> */}
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
              <Grid item xs={3} />
              </Grid>
            </Grid>
            <Grid item xs={12}>
                { props.isAutoRefreshDisabled ? 
                  null
                : ( 
                  <SwitchComponent type="regular" defaultValue={props.isAutoRefresh} switchHandler={() => onTriggerAutoUpdate()} />
                  )
                }
            </Grid>
          </Grid>
        ) : null
      }
    </TableContainer>
  )
}

export default ScreendoxTable;
