using System;
using System.Collections.Generic;
using DefaultNamespace.Gameplay.PainObject;
using DefaultNamespace.HUD.Core;
using TMPro;

namespace DefaultNamespace.HUD.HUDScripts.HUDLoad
{
    public class LoadHUDController : BaseHUDController<LoadHUDView>
    {
        private PaintObjectService _paintObjectService;

        public LoadHUDController(PaintObjectService paintObjectService)
        {
            _paintObjectService = paintObjectService;
        }

        public override PrefabName HUDItemName => PrefabName.HUDLoad;

        protected override void OnShow()
        {
            base.OnShow();
            TargetView.OnApplyButtonClicked += ApplyButtonClick;
            _paintObjectService.OnSave += UpdateDropdownValues;
            UpdateDropdownValues();
        }

        protected override void OnHide()
        {
            base.OnHide();
            _paintObjectService.OnSave -= UpdateDropdownValues;
            TargetView.OnApplyButtonClicked -= ApplyButtonClick;
        }

        private void UpdateDropdownValues()
        {
            var dataValues = _paintObjectService.GetSavesIndexes();
            if (dataValues.Count == 0)
            {
                TargetView.ToggleContainer(false);
                return;
            }

            TargetView.ToggleContainer(true);
            var options  = new List<TMP_Dropdown.OptionData>();
            foreach (var data in dataValues) options.Add(new TMP_Dropdown.OptionData(data.ToString()));
            
            TargetView.UpdateDropdownItem(options);
            
        }

        private void ApplyButtonClick()
        {
            var selectedValue = Int32.Parse(TargetView.SelectedValue);
            _paintObjectService.LoadSave(selectedValue);
        }
    }
}