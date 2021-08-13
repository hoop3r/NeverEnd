using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Engine;

namespace NeverEnd
{
    public partial class NeverEnd : Form
    {
        private Player _player;
        private Monster _currentMonster;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";

        public NeverEnd()
        {
            InitializeComponent();

            if (File.Exists(PLAYER_DATA_FILE_NAME))
            {
                _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
            }
            else
            {
                _player = Player.CreateDefaultPlayer();
            }


            MoveTo(_player.CurrentLocation);

            UpdatePlayerStats();
        }


        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToNorth);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToEast);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToWest);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.CurrentLocation.LocationToSouth);
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;
            int damageToMonster = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);

            _currentMonster.CurrentHitPoints -= damageToMonster;

            rtbMessages.Text += "Вы нанесли " + damageToMonster.ToString() + " урона " + _currentMonster.Name + Environment.NewLine;

            if (_currentMonster.CurrentHitPoints <= 0)
            {
                rtbMessages.Text += Environment.NewLine;
                rtbMessages.Text += "Вы победили " + _currentMonster.Name + Environment.NewLine;

                _player.ExperiencePoints += _currentMonster.RewardExperiencePoints;
                rtbMessages.Text += "Вы получили " + _currentMonster.RewardExperiencePoints.ToString() + " EXP." + Environment.NewLine;

                _player.Gold += _currentMonster.RewardGold;
                rtbMessages.Text += "Вы получили " + _currentMonster.RewardGold.ToString() + " золота." + Environment.NewLine;

                List<InventoryItem> lootedItems = new List<InventoryItem>();

                foreach (LootItem lootItem in _currentMonster.LootTable)
                {
                    if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }

                // Если никакие предметы не были выбраны случайным образом, добавляем предметы лута по умолчанию.
                if (lootedItems.Count == 0)
                {
                    foreach (LootItem lootItem in _currentMonster.LootTable)
                    {
                        if (lootItem.IsDefaultItem)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                        }
                    }
                }

                // Добавление лута в инвнт
                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    _player.AddItemToInventory(inventoryItem.Details);

                    if (inventoryItem.Quantity == 1)
                    {
                        rtbMessages.Text += "Вы получили " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.Name + Environment.NewLine + ".";
                    }
                    else
                    {
                        rtbMessages.Text += "Вы получили " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.NamePlural + Environment.NewLine + ".";
                    }
                }

                lblHitPoints.Text = _player.CurrentHitPoints.ToString();
                lblGold.Text = _player.Gold.ToString();
                lblExperience.Text = _player.ExperiencePoints.ToString();
                lblLevel.Text = _player.Level.ToString();

                UpdatePlayerStats();
                UpdateInventoryListInUI();
                UpdateWeaponListInUI();
                UpdatePotionListInUI();

                rtbMessages.Text += Environment.NewLine;
                MoveTo(_player.CurrentLocation);
            }
            else
            {
                int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

                rtbMessages.Text += _currentMonster.Name + " нанесла вам " + damageToPlayer.ToString() + " урона." + Environment.NewLine;

                _player.CurrentHitPoints -= damageToPlayer;

                lblHitPoints.Text = _player.CurrentHitPoints.ToString();

                if (_player.CurrentHitPoints <= 0)
                {
                    MessageBox.Show("Вас убили... Постарайтесь в следующие раз получше)");
                    rtbMessages.Text += _currentMonster.Name + " убил вас." + Environment.NewLine;

                    // Отправляет игрока домой
                    MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                }
            }

            UpdatePlayerStats();
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            // Выбираем определенное зелья из КомбоБокса
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

            // Добавляет ХП игроку
            _player.CurrentHitPoints = (_player.CurrentHitPoints + potion.AmountToHeal);

            // ХП не может быть выше максимального ХП
            if (_player.CurrentHitPoints > _player.MaximumHitPoints)
            {
                _player.CurrentHitPoints = _player.MaximumHitPoints;
            }

            // Убрать зелье
            foreach (InventoryItem ii in _player.Inventory)
            {
                if (ii.Details.ID == potion.ID)
                {
                    ii.Quantity--;
                    break;
                }
            }

            rtbMessages.Text += "You drink a " + potion.Name + Environment.NewLine;

            // Ход монстра атаковать

            // Расчитываем урон нанесенный игроку
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

            rtbMessages.Text += _currentMonster.Name + " нанесла вам " + damageToPlayer.ToString() + " урона." + Environment.NewLine;

            _player.CurrentHitPoints -= damageToPlayer;

            if (_player.CurrentHitPoints <= 0)
            {
                rtbMessages.Text += _currentMonster.Name + " убил вас." + Environment.NewLine;
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }

            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();
        }
        private void UpdatePlayerStats()
        {
            // Обновляем информацию об игроке и элементы управления инвентарем
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }
        private void MoveTo(Location newLocation)
        {
            //Имеет ли данная локацию требуемый предмет
            if (!_player.HasRequiredItemToEnterThisLocation(newLocation))
            {
                rtbMessages.Text += "Вы должны иметь " + newLocation.ItemRequiredToEnter.Name + ", чтобы войти на эту локацию." + Environment.NewLine;
                return;
            }

            // Обновляет текующую локацию игрока
            _player.CurrentLocation = newLocation;

            // Показывает/прячет кнопки перемещения
            btnNorth.Visible = newLocation.LocationToNorth != null;
            btnEast.Visible = newLocation.LocationToEast != null;
            btnSouth.Visible = newLocation.LocationToSouth != null;
            btnWest.Visible = newLocation.LocationToWest != null;

            // Показывает текущую локацию и описание к локации
            rtbLocation.Text = newLocation.Name + Environment.NewLine;
            rtbLocation.Text += newLocation.Description + Environment.NewLine;

            // Полностью излечивает игрока
            _player.CurrentHitPoints = _player.MaximumHitPoints;

            // Обновляет HP в UI
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();

            // Есть ли в этой локации квест
            if (newLocation.QuestAvailableHere != null)
            {
                // Смотрим есть ли у игрока этот квест и выполнен ли он
                bool playerAlreadyHasQuest = _player.HasThisQuest(newLocation.QuestAvailableHere);
                bool playerAlreadyCompletedQuest = _player.CompletedThisQuest(newLocation.QuestAvailableHere);

                // Смотрим есть ли квест у игрока
                if (playerAlreadyHasQuest)
                {
                    // Если игрок не выполнил ещё квест
                    if (!playerAlreadyCompletedQuest)
                    {
                        // Смотрит, есть ли у игрока вещи для завершения квеста
                        bool playerHasAllItemsToCompleteQuest = _player.HasAllQuestCompletionItems(newLocation.QuestAvailableHere);

                        // У игрока есть все вещи для завершения квеста
                        if (playerHasAllItemsToCompleteQuest)
                        {
                            // Показывает сообщение
                            rtbMessages.Text += Environment.NewLine;
                            rtbMessages.Text += "Вы получили новый квест: '" + newLocation.QuestAvailableHere.Name + "'." + Environment.NewLine;

                            // Убирает квестовые вещи из инвенторя
                            _player.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);


                            // Нагрда за квест
                            rtbMessages.Text += "Вы получите: " + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " опыта" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() + " золота" + Environment.NewLine;
                            rtbMessages.Text += newLocation.QuestAvailableHere.RewardItem.Name + Environment.NewLine;
                            rtbMessages.Text += Environment.NewLine;

                            _player.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
                            _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                            // Добавляет награду за выполнение квеста
                            _player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);

                            // Отмечает квест выполненым
                            _player.MarkQuestCompleted(newLocation.QuestAvailableHere);
                        }
                    }
                }
                else
                {
                    // Если у игрока нет этого квеста, то выводится сообщение
                    rtbMessages.Text += "Вы получили новый квест: " + newLocation.QuestAvailableHere.Name + "." + Environment.NewLine;
                    rtbMessages.Text += newLocation.QuestAvailableHere.Description + Environment.NewLine;
                    rtbMessages.Text += "Чтобы выполнить его, вернитесь с: " + Environment.NewLine;
                    foreach (QuestCompletionItem qci in newLocation.QuestAvailableHere.QuestCompletionItems)
                    {
                        if (qci.Quantity == 1)
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.Name + Environment.NewLine;
                        }
                        else
                        {
                            rtbMessages.Text += qci.Quantity.ToString() + " " + qci.Details.NamePlural + Environment.NewLine;
                        }
                    }
                    rtbMessages.Text += Environment.NewLine;

                    // Добавляет квест в список квестов игрока
                    _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
                }
            }

            // Есть ли в этой локации монстор?
            if (newLocation.MonsterLivingHere != null)
            {
                rtbMessages.Text += "Вы видите " + newLocation.MonsterLivingHere.Name + Environment.NewLine + ".";

                // Создает нового монстра, используя значения из стандартного World.Monster списка
                Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);

                _currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage,
                    standardMonster.RewardExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints);

                foreach (LootItem lootItem in standardMonster.LootTable)
                {
                    _currentMonster.LootTable.Add(lootItem);
                }

                cboWeapons.Visible = true;
                cboPotions.Visible = true;
                btnUseWeapon.Visible = true;
                btnUsePotion.Visible = true;
            }
            else
            {
                _currentMonster = null;

                cboWeapons.Visible = false;
                cboPotions.Visible = false;
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
            }

            // Обновляет инвентарь игрока
            UpdateInventoryListInUI();

            // Обновляет квесты игрока
            UpdateQuestListInUI();

            // Обновляет оружие в КомбоБокс оружие
            UpdateWeaponListInUI();

            // Обновляет зелья в КомбоБокс зелья
            UpdatePotionListInUI();

            UpdatePlayerStats();
        }
        private void UpdateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;

            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Название";
            dgvInventory.Columns[0].Width = 140;
            dgvInventory.Columns[1].Name = "Количество";
            dgvInventory.Columns[1].Width = 90;

            dgvInventory.Rows.Clear();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { inventoryItem.Details.Name, inventoryItem.Quantity.ToString() });
                }
            }
        }
        private void UpdateQuestListInUI()
        {
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Название";
            dgvQuests.Columns[0].Width = 140;
            dgvQuests.Columns[1].Name = "Выполнено?";
            dgvQuests.Columns[1].Width = 90;

            dgvQuests.Rows.Clear();

            foreach (PlayerQuest playerQuest in _player.Quests)
            {
                dgvQuests.Rows.Add(new[] { playerQuest.Details.Name, playerQuest.IsCompleted.ToString() });
            }
        }
        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is Weapon)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.Details);
                    }
                }
            }

            if (weapons.Count == 0)
            {
                // The player doesn't have any weapons, so hide the weapon combobox and "Use" button
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.SelectedIndexChanged -= cboWeapons_SelectedIndexChanged;
                cboWeapons.DataSource = weapons;
                cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";

                if (_player.CurrentWeapon != null)
                {
                    cboWeapons.SelectedItem = _player.CurrentWeapon;
                }
                else
                {
                    cboWeapons.SelectedIndex = 0;
                }
            }
        }
        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem inventoryItem in _player.Inventory)
            {
                if (inventoryItem.Details is HealingPotion)
                {
                    if (inventoryItem.Quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.Details);
                    }
                }
            }

            if (healingPotions.Count == 0)
            {
                // У игрока нет зельев, прячем КомбоБокс зелия и кнопку использовать зелье
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";

                cboPotions.SelectedIndex = 0;
            }
        }

        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player.CurrentWeapon = (Weapon)cboWeapons.SelectedItem;
        }

        private void cboPotions_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void picloc_Click(object sender, EventArgs e)
        {
            //switch (_player.CurrentLocation)
            //{
            //    case :
            //        pictureBox1.Image = ;
            //        break;
            //    case 2:
            //        pictureBox1.Image = ;
            //        break;
            //    case 3:
            //        pictureBox1.Image = ;
            //        break;
            //}
        }
    }
}
